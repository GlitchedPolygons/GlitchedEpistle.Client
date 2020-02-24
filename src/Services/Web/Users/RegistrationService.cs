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
using System.Threading.Tasks;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Settings;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.ServerHealth;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.KeyExchange;
using GlitchedPolygons.Services.Cryptography.Asymmetric;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Service interface implementation for creating new users on the backend.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly IAppSettings appSettings;
        private readonly IKeyExchange keyExchange;
        private readonly IAsymmetricKeygenRSA keygen;
        private readonly IServerConnectionTest connectionTest;

        private static readonly RSAKeySize RSA_KEY_SIZE = RSAKeySize.RSA4096;

        private Task<ValueTuple<string, string>> keyGenerationTask;
        
#pragma warning disable 1591
        public RegistrationService(IAsymmetricKeygenRSA keygen, IServerConnectionTest connectionTest, ILogger logger, IUserService userService, IAppSettings appSettings, IKeyExchange keyExchange)
        {
            this.logger = logger;
            this.keygen = keygen;
            this.userService = userService;
            this.appSettings = appSettings;
            this.keyExchange = keyExchange;
            this.connectionTest = connectionTest;

            keyGenerationTask = Task.Run(() => keygen.GenerateKeyPair(RSA_KEY_SIZE));
        }
#pragma warning restore 1591

        /// <summary>
        /// Submits a user registration request to the Epistle backend and returns the resulting status code.<para> </para>
        /// If the user creation succeeded, the created user's data is applied to the currently active session <see cref="User"/>.
        /// The meaning of the returned status codes is as follows:<para> </para>
        /// 0 = Success! The user was created and the related data was loaded into session <see cref="User"/>.<para> </para>
        /// 1 = Connection to the Epistle server could not be established.<para> </para>
        /// 2 = RSA Key generation failed/incomplete.<para> </para>
        /// 3 = User registration failed server-side.<para> </para>
        /// 4 = User registration failed client-side.<para> </para>
        /// </summary>
        /// <param name="password">The user's password (NOT the SHA512!)</param>
        /// <param name="userCreationSecret">The backend's user creation secret.</param>
        /// <returns>A tuple containing the resulting status code and (eventually) the <see cref="UserCreationResponseDto"/></returns>
        public async Task<ValueTuple<int, UserCreationResponseDto>> CreateUser(string password, string userCreationSecret)
        {
            if (!await connectionTest.TestConnection().ConfigureAwait(false))
            {
                return (1, null);
            }

            (string, string) keyPair = await keyGenerationTask.ConfigureAwait(false);
            
            keyGenerationTask = Task.Run(() => keygen.GenerateKeyPair(RSA_KEY_SIZE));

            if (keyPair.Item1.NullOrEmpty() || keyPair.Item2.NullOrEmpty())
            {
                return (2, null);
            }

            string publicKeyPem = keyPair.Item1;
            string privateKeyPem = keyPair.Item2;
            
            try
            {
                var userCreationResponse = await userService.CreateUser(new UserCreationRequestDto
                {
                    PasswordSHA512 = password.SHA512(),
                    CreationSecret = userCreationSecret,
                    PublicKey = await keyExchange.CompressPublicKeyAsync(publicKeyPem).ConfigureAwait(false),
                    PrivateKey = await keyExchange.EncryptAndCompressPrivateKeyAsync(privateKeyPem, password).ConfigureAwait(false),
                }).ConfigureAwait(false);

                if (userCreationResponse is null)
                {
                    logger?.LogError("The user creation process failed server-side. Reason unknown; please make an admin check out the server's log files!");
                    return (3, null);
                }

                appSettings.LastUserId = userCreationResponse.Id;
                
                // Handle this event back in the client UI,
                // since it's there where the backup codes + 2FA secret (QR) will be displayed.
                logger?.LogMessage($"Created user {userCreationResponse.Id}.");
                return (0, userCreationResponse);
            }
            catch (Exception e)
            {
                logger?.LogError($"The user creation process failed. Thrown exception: {e}");
                return (4, null);
            }
        }
    }
}
