/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

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
using System.IO;
using System.Web;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Settings;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.KeyExchange;
using GlitchedPolygons.Services.Cryptography.Asymmetric;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Service interface implementation for submitting messages to the Epistle backend.
    /// </summary>
    public class MessageSender : IMessageSender
    {
        private readonly User user;
        private readonly IKeyExchange keyExchange;
        private readonly IUserService userService;
        private readonly IUserSettings userSettings;
        private readonly IConvoService convoService;
        private readonly IMessageCryptography crypto;
        private readonly IAsymmetricCryptographyRSA rsa;
        private readonly IConvoPasswordProvider convoPasswordProvider;
        private static readonly char[] MSG_TRIM_CHARS = { '\n', '\r', '\t' };
        
        /// <summary>
        /// The maximum allowed file size for a convo's message (currently 32MB).
        /// </summary>
        public const long MAX_FILE_SIZE_BYTES = 33_554_432;
        
#pragma warning disable 1591
        public MessageSender(User user, IUserService userService, IConvoPasswordProvider convoPasswordProvider, IConvoService convoService, IAsymmetricCryptographyRSA rsa, IMessageCryptography crypto, IUserSettings userSettings, IKeyExchange keyExchange)
        {
            this.rsa = rsa;
            this.user = user;
            this.crypto = crypto;
            this.keyExchange = keyExchange;
            this.userService = userService;
            this.userSettings = userSettings;
            this.convoService = convoService;
            this.convoPasswordProvider = convoPasswordProvider;
        }
#pragma warning restore 1591
        
        /// <summary>
        /// Sends a text message to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into (will use the <see cref="Convo.Id"/> and other credentials for establishing a connection to the Epistle server).</param>
        /// <param name="message"></param>
        /// <returns>Whether the message submission was successful or failed.</returns>
        public async Task<bool> PostText(Convo convo, string message)
        {
            if (convo is null || message.NullOrEmpty() || message.Length > MAX_FILE_SIZE_BYTES)
            {
                return false;
            }
            
            var sb = new StringBuilder(message.Length);
            sb.Append("TEXT=").Append(message.TrimEnd(MSG_TRIM_CHARS).TrimStart(MSG_TRIM_CHARS));
            
            return await PostMessageToConvo(convo, sb.ToString()).ConfigureAwait(false);
        }

        /// <summary>
        /// Submits a file to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into (will use the <see cref="Convo.Id"/> and other credentials for establishing a connection to the Epistle server).</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileBytes">File <c>byte[]</c> </param>
        /// <returns>Whether the message could be submitted successfully to the backend or not.</returns>
        public async Task<bool> PostFile(Convo convo, string fileName, byte[] fileBytes)
        {
            if (fileBytes.LongLength > MAX_FILE_SIZE_BYTES || fileName.NullOrEmpty() || fileBytes.NullOrEmpty() || fileName.Contains("///"))
            {
                return false;
            }
            
            var sb = new StringBuilder(fileBytes.Length * 3 / 2);
            sb.Append("FILE=").Append(fileName).Append("///BASE64=").Append(Convert.ToBase64String(fileBytes));
            
            return await PostMessageToConvo(convo, sb.ToString()).ConfigureAwait(false);
        }

        /// <summary>
        /// Submits a message to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into.</param>
        /// <param name="message">The message body to encrypt and post.</param>
        /// <returns>Whether the message could be submitted successfully or not.</returns>
        private async Task<bool> PostMessageToConvo(Convo convo, string message)
        {
            if (message.NullOrEmpty())
            {
                return false;
            }
            
            // Get the keys of all convo participants here.
            IDictionary<string, string> publicKeys = await userService.GetUserPublicKeys(user.Id, convo.GetParticipantIdsCommaSeparated(), user.Token.Item2).ConfigureAwait(false);
            
            if (publicKeys is null)
            {
                return false;
            }

            // Encrypt the message for every convo participant individually.
            var concurrentDictionary = new ConcurrentDictionary<string,string>();
            
            Parallel.ForEach(publicKeys, kvp =>
            {
                (string userId, string userPublicKey) = kvp;
                if (userId.NotNullNotEmpty() && userPublicKey.NotNullNotEmpty())
                {
                    string decompressedKey = keyExchange.DecompressPublicKey(userPublicKey);
                    string encryptedMessage = crypto.EncryptMessage(message, decompressedKey);
                    concurrentDictionary.TryAdd(userId, encryptedMessage);
                }
            });

            try
            {
                var postParamsDto = new PostMessageParamsDto
                {
                    SenderName = userSettings.Username,
                    ConvoId = convo.Id,
                    ConvoPasswordSHA512 = convoPasswordProvider.GetPasswordSHA512(convo.Id),
                    MessageBodiesJson = JsonSerializer.Serialize(concurrentDictionary),
                };

                var body = new EpistleRequestBody
                {
                    UserId = user.Id,
                    Auth = user.Token.Item2,
                    Body = JsonSerializer.Serialize(postParamsDto)
                };

                return await convoService.PostMessage(body.Sign(rsa, user.PrivateKeyPem)).ConfigureAwait(false);
            }
            catch
            {
                return false;
            }
        }
    }
}
