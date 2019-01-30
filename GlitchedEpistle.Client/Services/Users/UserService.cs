using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;
using Newtonsoft.Json;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Constants;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Users
{
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE_API);
        private static readonly JsonSerializerSettings JSON_SERIALIZER_SETTINGS = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };

        /// <inheritdoc/>
        public async Task<string> Login(string userId, string passwordSHA512, string totp)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("users/login", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(passwordSHA512), passwordSHA512);
            request.AddQueryParameter(nameof(totp), totp);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Content : null;
        }

        /// <inheritdoc/>
        public async Task<bool> Validate2FA(string userId, string totp)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("users/login/2fa", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(totp), totp);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <inheritdoc/>
        public async Task<DateTime?> GetUserExpirationUTC(string userId)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"users/exp/{userId}", UriKind.Relative)
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
                resource: new Uri($"users/get-public-key/{userIds}", UriKind.Relative)
            );
            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var keys = JsonConvert.DeserializeObject<List<Tuple<string, string>>>(response.Content, JSON_SERIALIZER_SETTINGS);
            return keys;
        }

        /// <inheritdoc/>
        public async Task<bool> ChangeUserPassword(string userId, string auth, string oldPw, string newPw)
        {
            var request = new RestRequest(
                method: Method.PUT,
                resource: new Uri($"users/change-pw/{userId}", UriKind.Relative)
            );
            request.AddQueryParameter(nameof(auth), auth);
            request.AddQueryParameter(nameof(oldPw), oldPw);
            request.AddQueryParameter(nameof(newPw), newPw);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <inheritdoc/>
        public async Task<UserCreationResponse> CreateUser(string passwordHash, string publicKeyXml, string creationSecret)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("users/create", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(passwordHash), passwordHash);
            request.AddQueryParameter(nameof(publicKeyXml), publicKeyXml);
            request.AddQueryParameter(nameof(creationSecret), creationSecret);

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UserCreationResponse>(response.Content, JSON_SERIALIZER_SETTINGS);
        }
    }
}
