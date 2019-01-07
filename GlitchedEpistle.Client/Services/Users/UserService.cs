using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Users
{
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly RestClient restClient = new RestClient("https://epistle.glitchedpolygons.com/");

        /// <inheritdoc/>
        public async Task<string> Login(string userId, string passwordSHA512)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("api/users/login", UriKind.Relative)
            );

            request.AddParameter(nameof(userId), userId);
            request.AddParameter(nameof(passwordSHA512), passwordSHA512);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Content : null;
        }

        /// <inheritdoc/>
        public async Task<DateTime?> GetUserExpirationUTC(string userId)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"api/users/exp/{userId}", UriKind.Relative)
            );

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode == HttpStatusCode.OK && DateTime.TryParse(response.Content, out var exp))
            {
                return exp;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<List<Tuple<string, string>>> GetUserPublicKeyXml(string userId, string userIds, string auth)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"api/users/get-public-key/{userIds}", UriKind.Relative)
            );
            request.AddParameter(nameof(userId), userId);
            request.AddParameter(nameof(auth), auth);

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var keys = JsonConvert.DeserializeObject<List<Tuple<string, string>>>(response.Content);
            return keys;
        }
    }
}
