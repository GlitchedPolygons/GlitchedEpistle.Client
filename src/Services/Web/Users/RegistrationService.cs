using System;
using System.Threading.Tasks;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.ServerHealth;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;
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
        private readonly IAsymmetricKeygenRSA keygen;
        private readonly IServerConnectionTest connectionTest;
        private static readonly RSAKeySize RSA_KEY_SIZE = new RSA4096();

#pragma warning disable 1591
        public RegistrationService(IAsymmetricKeygenRSA keygen, IServerConnectionTest connectionTest, ILogger logger, IUserService userService)
        {
            this.logger = logger;
            this.keygen = keygen;
            this.userService = userService;
            this.connectionTest = connectionTest;
        }
#pragma warning restore 1591

        /// <summary>
        /// Submits a user registration request to the Epistle backend and returns the resulting status code.<para> </para>
        /// If the user creation succeeded, the created user's data is applied to the currently active session <see cref="User"/>.
        /// The meaning of the returned status codes is as follows:<para> </para>
        /// 0 = Success! The user was created and the related data was loaded into session <see cref="User"/>.<para> </para>
        /// 1 = Connection to the Epistle server could not be established.<para> </para>
        /// 2 = RSA Key generation failed/incomplete.<para> </para>
        /// 3 = User registration failed server-side.
        /// 4 = User registration failed client-side.
        /// </summary>
        /// <param name="password">The user's password (NOT the SHA512!)</param>
        /// <param name="userCreationSecret">The backend's user creation secret.</param>
        /// <returns>A tuple containing the resulting status code and (eventually) the <see cref="UserCreationResponseDto"/></returns>
        public async Task<Tuple<int, UserCreationResponseDto>> CreateUser(string password, string userCreationSecret)
        {
            if (!await connectionTest.TestConnection())
            {
                return new Tuple<int, UserCreationResponseDto>(1, null);
            }

            Tuple<string, string> keyPair = await keygen.GenerateKeyPair(RSA_KEY_SIZE);

            if (keyPair is null || keyPair.Item1.NullOrEmpty() || keyPair.Item2.NullOrEmpty())
            {
                return new Tuple<int, UserCreationResponseDto>(2, null);
            }

            try
            {
                string publicKeyPem = keyPair.Item1;
                string privateKeyPem = keyPair.Item2;

                var userCreationResponse = await userService.CreateUser(new UserCreationRequestDto
                {
                    PasswordSHA512 = password.SHA512(),
                    CreationSecret = userCreationSecret,
                    PublicKey = KeyExchangeUtility.CompressPublicKey(publicKeyPem),
                    PrivateKey = KeyExchangeUtility.EncryptAndCompressPrivateKey(privateKeyPem, password),
                });

                if (userCreationResponse is null)
                {
                    logger?.LogError("The user creation process failed server-side. Reason unknown; please make an admin check out the server's log files!");
                    return new Tuple<int, UserCreationResponseDto>(3, null);
                }

                // Handle this event back in the main view model,
                // since it's there where the backup codes + 2FA secret (QR) will be shown.
                logger?.LogMessage($"Created user {userCreationResponse.Id}.");
                return new Tuple<int, UserCreationResponseDto>(0, userCreationResponse);
            }
            catch (Exception e)
            {
                logger?.LogError($"The user creation process failed. Thrown exception: {e}");
                return new Tuple<int, UserCreationResponseDto>(4, null);
            }
        }
    }
}
