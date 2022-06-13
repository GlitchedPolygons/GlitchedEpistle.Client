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
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;
using GlitchedPolygons.Services.Cryptography.Asymmetric;

using RestSharp;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Service interface implementation for getting and uploading user profile pictures.
    /// </summary>
    public class ProfilePictureChanger : EpistleWebApiService, IProfilePictureChanger, IDisposable
    {
        private readonly User user;
        private readonly IAsymmetricCryptographyRSA rsa;
        private RestClient restClient = new RestClient(UrlUtility.EpistleAPI_v1);
        
#pragma warning disable 1591
        public ProfilePictureChanger(User user, IAsymmetricCryptographyRSA rsa)
        {
            this.rsa = rsa;
            this.user = user;
            UrlUtility.ChangedEpistleServerUrl += UrlUtility_ChangedEpistleServerUrl;
        }
        
        ~ProfilePictureChanger()
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
            var previousRestClient = restClient;
            restClient = new RestClient(UrlUtility.EpistleAPI_v1);
            previousRestClient.Dispose();
        }
        
        /// <summary>
        /// Gets one or more user's profile picture from the server.
        /// </summary>
        /// <param name="userIds">The userIds whose profile picture you want to retrieve (comma-separated if more than one; should contain NO whitespaces).</param>
        /// <returns>The deserialized JSON of userId-profilePictureBase64 tuples. E.g.: { "userId1": "base64pic1", "userId2": "base64pic2" }</returns>
        public async Task<IDictionary<string, string>> GetUserProfilePictures(string userIds)
        {
            if (user is null || user.Id.NullOrEmpty() || user.Token is null || user.Token.Item2.NullOrEmpty() || userIds.NullOrEmpty())
            {
                return null;
            }

            try
            {
                var request = new RestRequest(
                    method: Method.Get,
                    resource: new Uri("users/pic", UriKind.Relative)
                );

                request.AddQueryParameter("userId", user.Id);
                request.AddQueryParameter("auth", user.Token.Item2);
                request.AddQueryParameter("userIds", userIds);

                RestResponse response = await restClient.ExecuteAsync(request).ConfigureAwait(false);
                if (response?.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                
                var json = JsonDocument.Parse(response.Content);
                var pics = new Dictionary<string, string>(8);
                foreach (var key in json.RootElement.EnumerateObject())
                {
                    pics.TryAdd(key.Name, key.Value.GetString());
                }
                return pics;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Uploads a new profile picture to the server. 
        /// </summary>
        /// <param name="totp">2FA Token.</param>
        /// <param name="pic">The profile picture (base64-encoded). Can be <c>null</c>.</param>
        /// <returns>Whether the update was accepted or failed.</returns>
        public async Task<bool> UpdateUserProfilePicture(string totp, string pic)
        {
            try
            {
                if (user is null || pic.Length > 512 * 512 || user.Id.NullOrEmpty() || user.Token is null || user.Token.Item2.NullOrEmpty() || totp.NullOrEmpty())
                {
                    return false;
                }

                var dto = new UserChangeProfilePictureRequestDto
                {
                    Totp = totp,
                    ProfilePicture = pic,
                    PasswordSHA512 = user.PasswordSHA512
                };

                var requestBody = new EpistleRequestBody
                {
                    UserId = user.Id,
                    Auth = user.Token?.Item2,
                    Body = JsonSerializer.Serialize(dto)
                };

                var request = EpistleRequest(requestBody.Sign(rsa, user.PrivateKeyPem), "users/pic", Method.Put);
                RestResponse response = await restClient.ExecuteAsync(request).ConfigureAwait(false);
                return response?.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}