using System;
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
    /// Implements the <see cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Convos.IConvoService" /> <c>interface</c>.
    /// </summary>
    /// <seealso cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Convos.IConvoService" />
    public class ConvoService : IConvoService
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE_API);

        public async Task<string> DownloadAttachment(string attachmentId, string convoId, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"convos/attachments/{attachmentId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(convoId), convoId);
            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.Content;
        }

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

        public async Task<bool> PostMessage(string convoId, string convoPasswordHash, string userId, string auth, string senderName, string messageBodiesJson)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri($"convos/{convoId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(convoPasswordHash), convoPasswordHash);
            request.AddQueryParameter(nameof(senderName), senderName);
            request.AddQueryParameter(nameof(messageBodiesJson), messageBodiesJson);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }

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
