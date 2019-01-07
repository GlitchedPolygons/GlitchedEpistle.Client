using System;
using System.Net;
using System.Threading.Tasks;

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
    }
}
