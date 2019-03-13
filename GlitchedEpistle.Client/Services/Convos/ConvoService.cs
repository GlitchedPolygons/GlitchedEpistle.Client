using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using GlitchedPolygons.GlitchedEpistle.Client.Constants;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos on the web API (remote).
    /// Implements the <see cref="IConvoService" /> <c>interface</c>.
    /// </summary>
    /// <seealso cref="IConvoService" />
    public class ConvoService : IConvoService
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE_API);

        /// <summary>
        /// Creates a new convo on the server.
        /// </summary>
        /// <param name="convoDto">The convo creation DTO.</param>
        /// <param name="userId">The user identifier (who's making the request).</param>
        /// <param name="auth">The authentication token (JWT).</param>
        /// <returns><c>null</c> if creation failed; the created <see cref="Convo" />'s unique id.</returns>
        public async Task<string> CreateConvo(ConvoCreationDto convoDto, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("convos/create", UriKind.Relative)
            );

            request.AddJsonBody(convoDto);
            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.Content;
        }

        /// <summary>
        /// Deletes a convo server-side.
        /// </summary>
        /// <param name="convoId">The <see cref="Convo" />'s identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (who's making the request; needs to be the convo's admin).</param>
        /// <param name="auth">The authentication JWT.</param>
        /// <returns>Whether deletion was successful or not.</returns>
        public async Task<bool> DeleteConvo(string convoId, string convoPasswordHash, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.DELETE,
                resource: new Uri($"convos/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }

        /// <summary>
        /// Posts a message to a <see cref="Convo" />.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="messageDto">The message post parameters (for the request body).</param>
        /// <returns>Whether the message was posted successfully or not.</returns>
        public async Task<bool> PostMessage(string convoId, PostMessageParamsDto messageDto)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri($"convos/{convoId}", UriKind.Relative)
            );

            request.AddJsonBody(messageDto);
            
            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }

        /// <summary>
        /// Gets a convo's metadata (description, timestamp, etc...).
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a participant of the convo).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>The convo's metadata wrapped into a DTO (<c>null</c> if something failed).</returns>
        public async Task<ConvoMetadataDto> GetConvoMetadata(string convoId, string convoPasswordHash, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"convos/meta/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful ? JsonConvert.DeserializeObject<ConvoMetadataDto>(response.Content) : null;
        }

        /// <summary>
        /// Gets the convo messages.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a convo participant).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="fromIndex">The index from which to start retrieving messages inclusively (e.g. starting from index 4 will include <c>convo.Messages[4]</c>).</param>
        /// <returns>The retrieved <see cref="Message" />s (<c>null</c> if everything is up to date or if something failed).</returns>
        public async Task<Message[]> GetConvoMessages(string convoId, string convoPasswordHash, string userId, string auth, int fromIndex = 0)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"convos/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);
            request.AddQueryParameter(nameof(fromIndex), fromIndex.ToString());

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful ? JsonConvert.DeserializeObject<Message[]>(response.Content) : null;
        }

        /// <summary>
        /// Gets the index of a message inside a <see cref="Convo" />.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier of who's making the request.</param>
        /// <param name="auth">The request authentication JWT.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>The <see cref="Message" />'s index integer; if something fails, <c>-1</c> is returned.</returns>
        public async Task<int> IndexOf(string convoId, string convoPasswordHash, string userId, string auth, string messageId)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"convos/indexof/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);
            request.AddQueryParameter(nameof(messageId), messageId);

            var response = await restClient.ExecuteTaskAsync(request);
            return int.TryParse(response.Content, out int i) ? i : -1;
        }

        /// <summary>
        /// Join a <see cref="Convo" />.
        /// </summary>
        /// <param name="convoId">The identifier of the <see cref="Convo" /> that you're trying to join.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user's identifier (who wants to join).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>Whether the <see cref="Convo" /> was joined successfully or not.</returns>
        public async Task<bool> JoinConvo(string convoId, string convoPasswordHash, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.PUT,
                resource: new Uri($"convos/join/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }

        /// <summary>
        /// Leave a <see cref="Convo" />.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="userId">The user identifier (who's leaving the convo).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns>Whether the <see cref="Convo" /> was left successfully or not.</returns>
        public async Task<bool> LeaveConvo(string convoId, string convoPasswordHash, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.PUT,
                resource: new Uri($"convos/leave/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }

        /// <summary>
        /// Kick a user from a conversation.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordHash">The convo's password hash.</param>
        /// <param name="convoAdminId">Your user id (you need to be a <see cref="Convo" />'s admin in order to kick people out of it).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="userIdToKick">The user id of who you're kicking out.</param>
        /// <param name="permaBan">If set to <c>true</c>, the kicked user won't be able to rejoin the convo permanently.</param>
        /// <returns>Whether the user was kicked out successfully or not.</returns>
        public async Task<bool> KickUser(string convoId, string convoPasswordHash, string convoAdminId, string auth, string userIdToKick, bool permaBan)
        {
            var request = new RestRequest(
                method: Method.PUT,
                resource: new Uri($"convos/{convoId}/kick/{userIdToKick}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(convoAdminId), convoAdminId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);
            request.AddQueryParameter(nameof(userIdToKick), userIdToKick);
            request.AddQueryParameter(nameof(permaBan), permaBan.ToString());

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }
    }
}