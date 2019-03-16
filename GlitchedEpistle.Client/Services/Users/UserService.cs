using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;
using Newtonsoft.Json;

using GlitchedPolygons.GlitchedEpistle.Client.Constants;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Users
{
    /// <summary>
    /// Service interface for logging into Glitched Epistle and receiving an auth token back from the Web API, as well as extending a user's expiration date.<para> </para>
    /// Implements the <see cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Users.IUserService" /> interface.
    /// </summary>
    /// <seealso cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Users.IUserService" />
    public class UserService : IUserService
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE_API);
        private static readonly JsonSerializerSettings JSON_SERIALIZER_SETTINGS = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };

        /// <summary>
        /// Logs the specified user in by authenticating the provided credentials
        /// (POST request to the Glitched Epistle Web API). If authentication is successful, a valid JWT is returned.
        /// That's needed for subsequent requests.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="passwordSHA512">The password hash (SHA-512).</param>
        /// <param name="totp">The 2FA code.</param>
        /// <returns>JWT <see langword="string" /> if auth was successful; <see langword="null" /> otherwise.</returns>
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

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="auth">The current authentication token.</param>
        /// <returns>If all goes well, you should receive your new, fresh auth token from the backend.</returns>
        public async Task<string> RefreshAuthToken(string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("users/login/refresh", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);

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

        /// <summary>
        /// Gets a <see cref="T:GlitchedPolygons.GlitchedEpistle.Client.Models.User" />'s expiration <see cref="T:System.DateTime" /> (in UTC).
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="T:GlitchedPolygons.GlitchedEpistle.Client.Models.User" />'s expiration <see cref="T:System.DateTime" /> in UTC; <see langword="null" /> if the user doesn't exist.</returns>
        public async Task<DateTime?> GetUserExpirationUTC(string userId)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"users/exp/{userId}", UriKind.Relative)
            );

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode == HttpStatusCode.OK && DateTime.TryParse(response.Content, out DateTime exp))
            {
                return exp;
            }

            return null;
        }

        /// <summary>
        /// Gets one or more users' public key XML (RSA key needed for encrypting messages for that user).
        /// </summary>
        /// <param name="userId">Your user identifier.</param>
        /// <param name="userIds">The user ids whose public key you want to retrieve (comma-separated).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns><c>List&lt;Tuple&lt;string, string&gt;&gt;</c> containing all of the user ids and their public key; <c>null</c> if the request failed in some way.</returns>
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

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="auth">The authentication token.</param>
        /// <param name="oldPw">The old password hash (SHA-512).</param>
        /// <param name="newPw">The new password hash (SHA-512).</param>
        /// <returns><c>bool</c> indicating whether the change was successful or not.</returns>
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userCreationDto">DTO containing user creation parameters (for the request body).</param>
        /// <returns>The user creation response data containing the TOTP secret to show only ONCE to the user (won't be stored)... or <c>null</c> if the creation failed.</returns>
        public async Task<UserCreationResponseDto> CreateUser(UserCreationDto userCreationDto)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("users/create", UriKind.Relative)
            );

            request.AddParameter("application/json", JsonConvert.SerializeObject(userCreationDto), ParameterType.RequestBody);

            var response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UserCreationResponseDto>(response.Content, JSON_SERIALIZER_SETTINGS);
        }
    }
}
