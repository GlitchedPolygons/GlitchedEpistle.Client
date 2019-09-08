/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

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

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Service interface for logging into Glitched Epistle
    /// and receiving an auth token back from the Web API.
    /// </summary>
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Logs the specified user in by authenticating the provided credentials with a POST request to the Glitched Epistle Web API.
        /// If authentication is successful, a valid authentication token <c>string</c> is returned along with the user's keypair.
        /// That's needed for subsequent requests.
        /// </summary>
        /// <param name="paramsDto">HTTP Request parameters wrapped into a DTO instance.</param>
        /// <returns><see cref="UserLoginSuccessResponseDto"/> if auth was successful; <c>null</c> otherwise.</returns>
        Task<UserLoginSuccessResponseDto> Login(UserLoginRequestDto paramsDto);

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="auth">The current authentication token.</param>
        /// <returns>If all goes well, you should receive your new, fresh auth token from the backend.</returns>
        Task<string> RefreshAuthToken(string userId, string auth);

        /// <summary>
        /// Validates the 2fa token.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="totp">The totp.</param>
        /// <returns>Whether the user 2FA authentication succeeded or not.</returns>
        Task<bool> Validate2FA(string userId, string totp);

        /// <summary>
        /// Gets the <see cref="Convo"/>s in which the specified <see cref="User"/> is involved (participant or creator thereof).
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="auth">Request authentication token.</param>
        /// <returns>The found convos.</returns>
        Task<IEnumerable<ConvoMetadataDto>> GetConvos(string userId, string auth);

        /// <summary>
        /// Gets one or more users' public key (RSA key needed for encrypting messages for that user).
        /// </summary>
        /// <param name="userId">Your user identifier.</param>
        /// <param name="userIds">The user ids whose public key you want to retrieve (comma-separated).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <returns><c>List&lt;Tuple&lt;string, string&gt;&gt;</c> containing all of the user ids and their public key; <c>null</c> if the request failed in some way.</returns>
        Task<List<Tuple<string, string>>> GetUserPublicKey(string userId, string userIds, string auth);

        /// <summary>
        /// Gets a user's (encrypted, gzipped and base-64 encoded) private key from the server.
        /// </summary>
        /// <param name="userId">The requesting user's id.</param>
        /// <param name="passwordSHA512">The requesting user's password hash (SHA512).</param>
        /// <param name="totp">Two-Factor Authentication token.</param>
        /// <returns><c>null</c> if retrieval failed; the key if the request was successful.</returns>
        Task<string> GetUserPrivateKey(string userId, string passwordSHA512, string totp);

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="requestBody">Request parameters DTO wrapped into an <see cref="EpistleRequestBody"/>.</param>
        /// <returns><c>bool</c> indicating whether the change was successful or not.</returns>
        Task<bool> ChangeUserPassword(EpistleRequestBody requestBody);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userCreationRequestDto">DTO containing user creation parameters (for the request body).</param>
        /// <returns>The user creation response data containing the TOTP secret to show only ONCE to the user (won't be stored)... or <c>null</c> if the creation failed.</returns>
        Task<UserCreationResponseDto> CreateUser(UserCreationRequestDto userCreationRequestDto);

        /// <summary>
        /// Deletes a user irreversibly from the backend's db.<para> </para>
        /// <paramref name="requestBody.Body"/> should directly be the unprocessed <see cref="User.PasswordSHA512"/>.
        /// </summary>
        /// <param name="requestBody">Request parameters DTO wrapped into an <see cref="EpistleRequestBody"/>.</param>
        /// <returns>Whether deletion was successful or not.</returns>
        Task<bool> DeleteUser(EpistleRequestBody requestBody);
    }
}
