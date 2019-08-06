#region
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos on the web API (remote).
    /// </summary>
    public interface IConvoService
    {
        /// <summary>
        /// Creates a new convo on the server.
        /// </summary>
        /// <param name="convoRequestDto">The convo creation DTO.</param>
        /// <param name="userId">The user identifier (who's making the request).</param>
        /// <param name="auth">The authentication token (JWT).</param>
        /// <returns><c>null</c> if creation failed; the created <see cref="Convo"/>'s unique id.</returns>
        Task<string> CreateConvo(ConvoCreationRequestDto convoRequestDto, string userId, string auth);

        /// <summary>
        /// Deletes a convo server-side.
        /// </summary>
        /// <param name="convoId">The <see cref="Convo"/>'s identifier.</param>
        /// <param name="totp">2FA token.</param>
        /// <param name="userId">The user identifier (who's making the request; needs to be the convo's admin).</param>
        /// <param name="auth">The authentication JWT.</param>
        /// <returns>Whether deletion was successful or not.</returns>
        Task<bool> DeleteConvo(string convoId, string totp, string userId, string auth);

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
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a participant of the convo).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>The convo's metadata wrapped into a DTO (<c>null</c> if something failed).</returns>
        Task<ConvoMetadataDto> GetConvoMetadata(string convoId, string convoPasswordSHA512, string userId, string auth);

        /// <summary>
        /// Changes a convo's metadata (description, title, etc...).<para> </para>
        /// The user making the request needs to be the <see cref="Convo"/>'s admin (Creator).<para> </para>
        /// If you're assigning a new admin, he needs to be a participant of the <see cref="Convo"/>, else you'll get a bad request returned from the web api.
        /// </summary>
        /// <param name="metadata">Request DTO containing authentication parameters + the data that needs to be changed (<c>null</c> fields will be ignored; fields with values will be updated and persisted into the server's db).</param>
        /// <returns>Whether the convo's metadata was changed successfully or not.</returns>
        Task<bool> ChangeConvoMetadata(ConvoChangeMetadataRequestDto metadata);

        /// <summary>
        /// Gets the convo messages.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a convo participant).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="tailId">The id of the tail message from which to start retrieving subsequent messages (e.g. starting from message id that evaluates to index 4 will not include <c>convo.Messages[4]</c>). Here you would pass the id of the last message the client already has. If this is null or empty, all messages will be retrieved!</param>
        /// <returns>The retrieved <see cref="Message" />s (<c>null</c> if everything is up to date or if something failed).</returns>
        Task<Message[]> GetConvoMessages(string convoId, string convoPasswordSHA512, string userId, string auth, string tailId = null);

        /// <summary>
        /// Gets the index of a message inside a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier of who's making the request.</param>
        /// <param name="auth">The request authentication JWT.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>The <see cref="Message"/>'s index integer; if something fails, <c>-1</c> is returned.</returns>
        Task<int> IndexOf(string convoId, string convoPasswordSHA512, string userId, string auth, string messageId);

        /// <summary>
        /// Join a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The identifier of the <see cref="Convo"/> that you're trying to join.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user's identifier (who wants to join).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>Whether the <see cref="Convo"/> was joined successfully or not.</returns>
        Task<bool> JoinConvo(string convoId, string convoPasswordSHA512, string userId, string auth);

        /// <summary>
        /// Leave a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="totp">2FA TOTP code.</param>
        /// <param name="userId">The user identifier (who's leaving the convo).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns>Whether the <see cref="Convo"/> was left successfully or not.</returns>
        Task<bool> LeaveConvo(string convoId, string totp, string userId, string auth);

        /// <summary>
        /// Kick a user from a conversation.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="convoAdminId">Your user id (you need to be a <see cref="Convo"/>'s admin in order to kick people out of it).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="userIdToKick">The user id of who you're kicking out.</param>
        /// <param name="permaBan">If set to <c>true</c>, the kicked user won't be able to rejoin the convo permanently.</param>
        /// <returns>Whether the user was kicked out successfully or not.</returns>
        Task<bool> KickUser(string convoId, string convoPasswordSHA512, string convoAdminId, string auth, string userIdToKick, bool permaBan);
    }
}
