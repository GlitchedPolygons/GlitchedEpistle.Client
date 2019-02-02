using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

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
        /// Gets one or more users' public key XML (RSA key needed for encrypting messages for that user).
        /// </summary>
        /// <param name="userId">Your user identifier.</param>
        /// <param name="userIds">The user ids whose public key you want to retrieve (comma-separated).</param>
        /// <param name="auth">The request authenticatication token.</param>
        /// <returns><c>List&lt;Tuple&lt;string, string&gt;&gt;</c> containing all of the user ids and their public key; <c>null</c> if the request failed in some way.</returns>
        Task<List<Tuple<string, string>>> GetUserPublicKeyXml(string userId, string userIds, string auth);

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="auth">The authentication token.</param>
        /// <param name="oldPw">The old password hash (SHA-512).</param>
        /// <param name="newPw">The new password hash (SHA-512).</param>
        /// <returns><c>bool</c> indicating whether the change was successful or not.</returns>
        Task<bool> ChangeUserPassword(string userId, string auth, string oldPw, string newPw);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="passwordHash">The user's password hash (SHA-512).</param>
        /// <param name="publicKeyXml">The user's public key XML (RSA key for encrypting messages for him).</param>
        /// <param name="creationSecret">The creation secret.</param>
        /// <returns>The user creation response data containing the TOTP secret to show only ONCE to the user (won't be stored)... or <c>null</c> if the creation failed.</returns>
        Task<UserCreationResponse> CreateUser(string passwordHash, string publicKeyXml, string creationSecret);
    }
}
