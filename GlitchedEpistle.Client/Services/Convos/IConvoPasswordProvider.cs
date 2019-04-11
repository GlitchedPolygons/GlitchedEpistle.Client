using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Interface for storing <see cref="Convo"/> password SHA512 strings during a session.
    /// </summary>
    public interface IConvoPasswordProvider
    {
        /// <summary>
        /// Gets a conversation's password SHA512 from the session's password provider.<para> </para>
        /// Returns <c>null</c> if the user has never accessed the convo during the session.
        /// </summary>
        /// <param name="convoId">The convo identifier.</param>
        /// <returns>The convo's password SHA512 <c>string</c>; <c>null</c> if the password was</returns>
        string GetPasswordSHA512(string convoId);

        /// <summary>
        /// Saves a convo's password SHA512 for the current app session for easy access.
        /// </summary>
        /// <param name="convoId">The convo identifier.</param>
        /// <param name="passwordSHA512">The password's SHA512.</param>
        void SetPasswordSHA512(string convoId, string passwordSHA512);

        /// <summary>
        /// Removes a convo password SHA512 from the session's cache.
        /// </summary>
        /// <param name="convoId">The convo identifier.</param>
        void RemovePasswordSHA512(string convoId);

        /// <summary>
        /// Clears all session-stored passwords.
        /// </summary>
        void Clear();
    }
}
