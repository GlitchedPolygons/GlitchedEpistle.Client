#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Users
{
    /// <summary>
    /// Service interface for logging into Glitched Epistle and receiving an auth token back from the Web API, as well as extending a user's expiration date.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Logs the specified user in by authenticating the provided credentials
        /// (POST request to the Glitched Epistle Web API). If authentication is successful, a valid JWT is returned.
        /// That's needed for subsequent requests.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="passwordSHA512">The password hash (SHA-512).</param>
        /// <param name="totp">The 2FA code.</param>
        /// <returns>JWT <see langword="string"/> if auth was successful; <see langword="null"/> otherwise.</returns>
        Task<string> Login(string userId, string passwordSHA512, string totp);

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="auth">The current authentication token.</param>
        /// <returns>If all goes well, you should receive your new, fresh auth token from the backend.</returns>
        Task<string> RefreshAuthToken(string userId, string auth);

        /// <summary>
        /// Validates the 2fa token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="totp">The totp.</param>
        /// <returns>Whether the user 2FA authentication succeeded or not.</returns>
        Task<bool> Validate2FA(string userId, string totp);

        /// <summary>
        /// Gets a <see cref="User"/>'s expiration <see cref="DateTime"/> (in UTC).
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="User"/>'s expiration <see cref="DateTime"/> in UTC; <see langword="null"/> if the user doesn't exist.</returns>
        Task<DateTime?> GetUserExpirationUTC(string userId);

        /// <summary>
        /// Gets the <see cref="Convo"/>s in which the specified <see cref="User"/> is involved (participant or creator thereof).
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="auth">Request authentication token.</param>
        /// <returns>The found convos.</returns>
        Task<IEnumerable<ConvoMetadataDto>> GetConvos(string userId, string auth);

        /// <summary>
        /// Gets one or more users' public key XML (RSA key needed for encrypting messages for that user).
        /// </summary>
        /// <param name="userId">Your user identifier.</param>
        /// <param name="userIds">The user ids whose public key you want to retrieve (comma-separated).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns><c>List&lt;Tuple&lt;string, string&gt;&gt;</c> containing all of the user ids and their public key; <c>null</c> if the request failed in some way.</returns>
        Task<List<Tuple<string, string>>> GetUserPublicKeyXml(string userId, string userIds, string auth);

        /// <summary>
        /// Gets a user's (encrypted, base-64 encoded) private key xml from the server.
        /// </summary>
        /// <param name="userId">The requesting user's id.</param>
        /// <param name="passwordSHA512">The requesting user's password hash (SHA512).</param>
        /// <param name="totp">Two-Factor Authentication token.</param>
        /// <returns><c>null</c> if retrieval failed; the key if the request was successful.</returns>
        Task<string> GetUserPrivateKeyXmlEncryptedBytesBase64(string userId, string passwordSHA512, string totp);

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="paramsDto">Request parameters DTO.</param>
        /// <returns><c>bool</c> indicating whether the change was successful or not.</returns>
        Task<bool> ChangeUserPassword(UserChangePasswordDto paramsDto);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userCreationDto">DTO containing user creation parameters (for the request body).</param>
        /// <returns>The user creation response data containing the TOTP secret to show only ONCE to the user (won't be stored)... or <c>null</c> if the creation failed.</returns>
        Task<UserCreationResponseDto> CreateUser(UserCreationDto userCreationDto);
    }
}
