/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Settings;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages;
using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.Services.Cryptography.Asymmetric;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Service interface implementation for submitting messages to the Epistle backend.
    /// </summary>
    public class MessageSender : IMessageSender
    {
        private readonly User user;
        private readonly IUserService userService;
        private readonly IUserSettings userSettings;
        private readonly IConvoService convoService;
        private readonly IMessageCryptography crypto;
        private readonly ICompressionUtilityAsync gzip;
        private readonly IAsymmetricCryptographyRSA rsa;
        private readonly IConvoPasswordProvider convoPasswordProvider;
        private static readonly char[] MSG_TRIM_CHARS = { '\n', '\r', '\t' };

        /// <summary>
        /// The maximum allowed file size for a convo's message.
        /// </summary>
        public const long MAX_FILE_SIZE_BYTES = 20971520;
        
#pragma warning disable 1591
        public MessageSender(User user, IUserService userService, IConvoPasswordProvider convoPasswordProvider, IConvoService convoService, IAsymmetricCryptographyRSA rsa, IMessageCryptography crypto, ICompressionUtilityAsync gzip, IUserSettings userSettings)
        {
            this.rsa = rsa;
            this.user = user;
            this.gzip = gzip;
            this.crypto = crypto;
            this.userSettings = userSettings;
            this.userService = userService;
            this.convoService = convoService;
            this.convoPasswordProvider = convoPasswordProvider;

            userSettings.Load();
        }
#pragma warning restore 1591
        
        /// <summary>
        /// Sends a text message to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into (will use the <see cref="Convo.Id"/> and other credentials for establishing a connection to the Epistle server).</param>
        /// <param name="message"></param>
        /// <returns>Whether the message submission was successful or failed.</returns>
        public Task<bool> PostText(Convo convo, string message)
        {
            return PostMessageToConvo(convo, new JObject
            {
                ["text"] = message.TrimEnd(MSG_TRIM_CHARS).TrimStart(MSG_TRIM_CHARS)
            }.ToString(Formatting.None));
        }

        /// <summary>
        /// Submits a file to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into (will use the <see cref="Convo.Id"/> and other credentials for establishing a connection to the Epistle server).</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileBytes">File <c>byte[]</c> </param>
        /// <returns>Whether the message could be submitted successfully to the backend or not.</returns>
        public Task<bool> PostFile(Convo convo, string fileName, byte[] fileBytes)
        {
            if (fileBytes.LongLength > MAX_FILE_SIZE_BYTES || fileName.NullOrEmpty() || fileBytes.NullOrEmpty())
            {
                return Task.FromResult(false);
            }

            return PostMessageToConvo(convo, new JObject
            {
                ["fileName"] = fileName,
                ["fileBase64"] = Convert.ToBase64String(fileBytes)
            }.ToString(Formatting.None));
        }

        /// <summary>
        /// Submits a message json body to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into.</param>
        /// <param name="messageBodyJson">The message body's JSON string.</param>
        /// <returns>Whether the message could be submitted successfully or not.</returns>
        private async Task<bool> PostMessageToConvo(Convo convo, string messageBodyJson)
        {
            // Get the keys of all convo participants here.
            List<Tuple<string, string>> keys = await userService.GetUserPublicKey(user.Id, convo.GetParticipantIdsCommaSeparated(), user.Token.Item2);

            // Encrypt the message for every convo participant individually
            // and put the result in a temporary concurrent dictionary.
            var encryptedMessagesBag = new ConcurrentDictionary<string, string>();

            Parallel.ForEach(keys, key =>
            {
                if (key != null && key.Item1.NotNullNotEmpty() && key.Item2.NotNullNotEmpty() && messageBodyJson.NotNullNotEmpty())
                {
                    encryptedMessagesBag[key.Item1] = crypto.EncryptMessage(messageBodyJson, KeyExchangeUtility.DecompressPublicKey(key.Item2));
                }
            });

            var postParamsDto = new PostMessageParamsDto
            {
                SenderName = userSettings.Username,
                ConvoId = convo.Id,
                ConvoPasswordSHA512 = convoPasswordProvider.GetPasswordSHA512(convo.Id),
                MessageBodiesJson = JsonConvert.SerializeObject(encryptedMessagesBag)
            };

            var body = new EpistleRequestBody
            {
                UserId = user.Id,
                Auth = user.Token.Item2,
                Body = await gzip.Compress(JsonConvert.SerializeObject(postParamsDto))
            };

            bool success = await convoService.PostMessage(body.Sign(rsa, user.PrivateKeyPem));
            return success;
        }
    }
}
