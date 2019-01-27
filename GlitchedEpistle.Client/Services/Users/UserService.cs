using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using Newtonsoft.Json;
using RestSharp;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Users
{
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly RestClient restClient = new RestClient("https://epistle.glitchedpolygons.com/");

        /// <inheritdoc/>
        public async Task<string> Login(string userId, string passwordSHA512, string totp)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("api/users/login", UriKind.Relative)
            );

            request.AddParameter(nameof(userId), userId);
            request.AddParameter(nameof(passwordSHA512), passwordSHA512);
            request.AddParameter(nameof(totp), totp);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Content : null;
        }

        /// <inheritdoc/>
        public async Task<bool> Validate2FA(string userId, string totp)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("api/users/login/2fa", UriKind.Relative)
            );

            request.AddParameter(nameof(userId), userId);
            request.AddParameter(nameof(totp), totp);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
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

        /// <inheritdoc/>
        public async Task<bool> ChangeUserPassword(string userId, string auth, string oldPw, string newPw)
        {
            var request = new RestRequest(
                method: Method.PUT,
                resource: new Uri($"api/users/change-pw/{userId}", UriKind.Relative)
            );
            request.AddParameter(nameof(auth), auth);
            request.AddParameter(nameof(oldPw), oldPw);
            request.AddParameter(nameof(newPw), newPw);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <inheritdoc/>
        public async Task<UserCreationResponse> CreateUser(string passwordHash, string publicKeyXml, string creationSecret)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("api/users/create", UriKind.Relative)
            );
            request.AddParameter(nameof(passwordHash), passwordHash);
            request.AddParameter(nameof(publicKeyXml), publicKeyXml);
            request.AddParameter(nameof(creationSecret), creationSecret);

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UserCreationResponse>(response.Content);
        }
    }
}
