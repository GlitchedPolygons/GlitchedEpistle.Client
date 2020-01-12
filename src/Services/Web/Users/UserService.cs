/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;

using Newtonsoft.Json;

using RestSharp;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Service interface for logging into Glitched Epistle and receiving an auth token back from the Web API.
    /// Implements the <see cref="IUserService" /> interface.
    /// </summary>
    /// <seealso cref="IUserService" />
    public class UserService : EpistleWebApiService, IUserService, IDisposable
    {
        private static readonly JsonSerializerSettings JSON_SERIALIZER_SETTINGS = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };
        private RestClient restClient = new RestClient(UrlUtility.EpistleAPI_v1);
        
#pragma warning disable 1591
        public UserService()
        {
            UrlUtility.ChangedEpistleServerUrl += UrlUtility_ChangedEpistleServerUrl;
        }

        ~UserService()
        {
            Dispose();
        }

        public void Dispose()
        {
            UrlUtility.ChangedEpistleServerUrl -= UrlUtility_ChangedEpistleServerUrl;
        }
#pragma warning restore 1591

        private void UrlUtility_ChangedEpistleServerUrl()
        {
            restClient = new RestClient(UrlUtility.EpistleAPI_v1);
        }
        
        /// <summary>
        /// Logs the specified user in by authenticating the provided credentials
        /// (POST request to the Glitched Epistle Web API).
        /// If authentication is successful, a valid JWT <c>string</c> is returned along with the user's keypair.
        /// That's needed for subsequent requests.
        /// </summary>
        /// <param name="paramsDto">HTTP Request parameters wrapped into a DTO instance.</param>
        /// <returns><see cref="UserLoginSuccessResponseDto"/> if auth was successful; <c>null</c> otherwise.</returns>
        public async Task<UserLoginSuccessResponseDto> Login(UserLoginRequestDto paramsDto)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("users/login", UriKind.Relative)
            );

            request.AddParameter("application/json", JsonConvert.SerializeObject(paramsDto), ParameterType.RequestBody);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);

            try
            {
                var r = JsonConvert.DeserializeObject<UserLoginSuccessResponseDto>(response.Content);
                return response.StatusCode == HttpStatusCode.OK ? r : null;
            }
            catch (Exception)
            {
                return null;
            }
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

            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Content : null;
        }

        /// <summary>
        /// Validates the 2fa token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="totp">The totp.</param>
        /// <returns>Whether the user 2FA authentication succeeded or not.</returns>
        public async Task<bool> Validate2FA(string userId, string totp)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("users/login/2fa", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(totp), totp);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets the <see cref="Convo"/>s in which the specified <see cref="User"/> is involved (participant or creator thereof).
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="auth">Request authentication token.</param>
        /// <returns>The found convos.</returns>
        public async Task<IEnumerable<ConvoMetadataDto>> GetConvos(string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"users/{userId}/convos", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(auth), auth);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Array.Empty<ConvoMetadataDto>();
            }

            try
            {
                var convos = JsonConvert.DeserializeObject<ConvoMetadataDto[]>(response.Content);
                return convos;
            }
            catch (Exception)
            {
                return Array.Empty<ConvoMetadataDto>();
            }
        }

        /// <summary>
        /// Gets one or more users' public key (RSA key needed for encrypting messages for that user).
        /// </summary>
        /// <param name="userId">Your user identifier.</param>
        /// <param name="userIds">The user ids whose public key you want to retrieve (comma-separated).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns><c>List&lt;Tuple&lt;string, string&gt;&gt;</c> containing all of the user ids and their public key; <c>null</c> if the request failed in some way.</returns>
        public async Task<List<Tuple<string, string>>> GetUserPublicKey(string userId, string userIds, string auth)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"users/get-public-key/{userIds}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            List<Tuple<string, string>> keys = JsonConvert.DeserializeObject<List<Tuple<string, string>>>(response.Content, JSON_SERIALIZER_SETTINGS);
            return keys;
        }

        /// <summary>
        /// Gets a user's (encrypted, compressed and base-64 encoded) private key from the server.
        /// </summary>
        /// <param name="userId">The requesting user's id.</param>
        /// <param name="passwordSHA512">The requesting user's password hash (SHA512).</param>
        /// <param name="totp">Two-Factor Authentication token.</param>
        /// <returns><c>null</c> if retrieval failed; the key if the request was successful.</returns>
        public async Task<string> GetUserPrivateKey(string userId, string passwordSHA512, string totp)
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri($"users/get-private-key/{userId}", UriKind.Relative)
            );

            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(passwordSHA512), passwordSHA512);
            request.AddQueryParameter(nameof(totp), totp);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Content : null;
        }

        /// <summary>
        /// Changes the user password.<para> </para>
        /// Remember that <paramref name="requestBody"/>'s field
        /// <see cref="EpistleRequestBody.Body"/> should be the <see cref="UserChangePasswordRequestDto"/>
        /// instance serialized into JSON and then compressed!
        /// </summary>
        /// <param name="requestBody">Request parameters DTO wrapped into an <see cref="EpistleRequestBody"/>.</param>
        /// <returns><c>bool</c> indicating whether the change was successful or not.</returns>
        public async Task<bool> ChangeUserPassword(EpistleRequestBody requestBody)
        {
            var request = EpistleRequest(requestBody, "users/change-pw", Method.PUT);
            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userCreationRequestDto">DTO containing user creation parameters (for the request body).</param>
        /// <returns>The user creation response data containing the TOTP secret to show only ONCE to the user (won't be stored)... or <c>null</c> if the creation failed.</returns>
        public async Task<UserCreationResponseDto> CreateUser(UserCreationRequestDto userCreationRequestDto)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("users/create", UriKind.Relative)
            );

            request.AddParameter("application/json", JsonConvert.SerializeObject(userCreationRequestDto), ParameterType.RequestBody);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UserCreationResponseDto>(response.Content, JSON_SERIALIZER_SETTINGS);
        }

        /// <summary>
        /// Deletes a user irreversibly from the backend's db.<para> </para>
        /// <paramref name="requestBody.Body"/> should directly be the unprocessed <see cref="User.PasswordSHA512"/>.
        /// </summary>
        /// <param name="requestBody">Request parameters DTO wrapped into an <see cref="EpistleRequestBody"/>.</param>
        /// <returns>Whether deletion was successful or not.</returns>
        public async Task<bool> DeleteUser(EpistleRequestBody requestBody)
        {
            var request = EpistleRequest(requestBody, "users/delete");
            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
