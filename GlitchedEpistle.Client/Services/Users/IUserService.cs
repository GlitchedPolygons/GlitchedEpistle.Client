using System;
using System.Threading.Tasks;
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
        /// <returns>JWT <see langword="string"/> if auth was successful; <see langword="null"/> otherwise.</returns>
        Task<string> Login(string userId, string passwordSHA512);

        /// <summary>
        /// Gets a <see cref="User"/>'s expiration <see cref="DateTime"/> (in UTC).
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="User"/>'s expiration <see cref="DateTime"/> in UTC; <see langword="null"/> if the user doesn't exist.</returns>
        Task<DateTime?> GetUserExpirationUTC(string userId);
    }
}
