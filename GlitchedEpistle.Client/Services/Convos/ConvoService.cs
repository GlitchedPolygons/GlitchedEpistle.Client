using System;
using System.Net;
using System.Threading.Tasks;

using RestSharp;
using GlitchedPolygons.GlitchedEpistle.Client.Constants;

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
    }
}
