namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Authentication
{
    /// <summary>
    /// Service interface for logging into Glitched Epistle and receiving an auth token back from the Web API.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Logs the specified user in by authenticating the provided credentials
        /// (POST request to the Glitched Epistle Web API). If authentication is successful, a valid JWT is returned.
        /// That's needed for subsequent requests.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="passwordSHA512">The password hash (SHA-512).</param>
        /// <returns>JWT <see langword="string"/></returns>
        string Login(string userId, string passwordSHA512);
    }
}
