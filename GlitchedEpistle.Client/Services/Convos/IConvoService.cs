using System.Threading.Tasks;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos on the web API (remote).
    /// </summary>
    public interface IConvoService
    {
        /// <summary>
        /// Creates a new convo on the server.
        /// </summary>
        /// <param name="convoDto">The convo creation DTO.</param>
        /// <param name="userId">The user identifier (who's making the request).</param>
        /// <param name="auth">The authentication token (JWT).</param>
        /// <returns><c>null</c> if creation failed; the created <see cref="Convo"/>'s unique id.</returns>
        Task<string> CreateConvo(ConvoCreationDto convoDto, string userId, string auth);

        /// <summary>
        /// Deletes a convo server-side.
        /// </summary>
        /// <param name="convoId">The <see cref="Convo"/>'s identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (who's making the request; needs to be the convo's admin).</param>
        /// <param name="auth">The authentication JWT.</param>
        /// <returns>Whether deletion was successful or not.</returns>
        Task<bool> DeleteConvo(string convoId, string convoPasswordHash, string userId, string auth);

        /// <summary>
        /// Posts a message to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="messageDto">The message post parameters (for the request body).</param>
        /// <returns>Whether the message was posted successfully or not.</returns>
        Task<bool> PostMessage(string convoId, PostMessageParamsDto messageDto);

        /// <summary>
        /// Gets a convo's metadata (description, timestamp, etc...).
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a participant of the convo).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>The convo's metadata wrapped into a DTO (<c>null</c> if something failed).</returns>
        Task<ConvoMetadataDto> GetConvoMetadata(string convoId, string convoPasswordHash, string userId, string auth);

        /// <summary>
        /// Gets the convo messages.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a convo participant).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="fromIndex">The index from which to start retrieving messages inclusively (e.g. starting from index 4 will include <c>convo.Messages[4]</c>).</param>
        /// <returns>The retrieved <see cref="Message"/>s (<c>null</c> if everything is up to date or if something failed).</returns>
        Task<Message[]> GetConvoMessages(string convoId, string convoPasswordHash, string userId, string auth, int fromIndex = 0);

        /// <summary>
        /// Gets the index of a message inside a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier of who's making the request.</param>
        /// <param name="auth">The request authentication JWT.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>The <see cref="Message"/>'s index integer; if something fails, <c>-1</c> is returned.</returns>
        Task<int> IndexOf(string convoId, string convoPasswordHash, string userId, string auth, string messageId);

        /// <summary>
        /// Join a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The identifier of the <see cref="Convo"/> that you're trying to join.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user's identifier (who wants to join).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>Whether the <see cref="Convo"/> was joined successfully or not.</returns>
        Task<bool> JoinConvo(string convoId, string convoPasswordHash, string userId, string auth);

        /// <summary>
        /// Leave a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (who's leaving the convo).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns>Whether the <see cref="Convo"/> was left successfully or not.</returns>
        Task<bool> LeaveConvo(string convoId, string convoPasswordHash, string userId, string auth);

        /// <summary>
        /// Kick a user from a conversation.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="convoAdminId">Your user id (you need to be a <see cref="Convo"/>'s admin in order to kick people out of it).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="userIdToKick">The user id of who you're kicking out.</param>
        /// <param name="permaBan">If set to <c>true</c>, the kicked user won't be able to rejoin the convo permanently.</param>
        /// <returns>Whether the user was kicked out successfully or not.</returns>
        Task<bool> KickUser(string convoId, string convoPasswordHash, string convoAdminId, string auth, string userIdToKick, bool permaBan);
    }
}
