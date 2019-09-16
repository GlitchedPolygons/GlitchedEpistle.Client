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

using System;
using System.Threading.Tasks;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Settings;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.ServerHealth;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// User login service.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly User user;
        private readonly IUserService userService;
        private readonly IAppSettings appSettings;
        private readonly IServerConnectionTest connectionTest;

#pragma warning disable 1591
        public LoginService(IServerConnectionTest connectionTest, IUserService userService, IAppSettings appSettings, User user)
        {
            this.user = user;
            this.appSettings = appSettings;
            this.userService = userService;
            this.connectionTest = connectionTest;
        }
#pragma warning restore 1591

        /// <summary>
        /// Logs a <see cref="User"/> in and applies the session token + key to the local <see cref="User"/> instance.<para> </para>
        /// The resulting status code has the following meaning:<para> </para>
        /// 0 = Successful login<para> </para>
        /// 1 = Connection to the Epistle server could not be established.<para> </para>
        /// 2 = Login failed (invalid credentials or returned jwt/key).<para> </para>
        /// 3 = Login succeeded server-side, but failed client-side (e.g. response's private key could not be decrypted, auth token could not be attributed to the <see cref="User"/> instance, etc...).
        /// </summary>
        /// <param name="userId">The user login id.</param>
        /// <param name="userPassword">The user's password (NOT its SHA512!).</param>
        /// <param name="totp">The 2-Factor Authentication token.</param>
        /// <returns>The status code (as described in the summary).</returns>
        public async Task<int> Login(string userId, string userPassword, string totp)
        {
            if (!await connectionTest.TestConnection())
            {
                return 1;
            }

            UserLoginSuccessResponseDto response = await userService.Login(new UserLoginRequestDto
            {
                UserId = userId,
                PasswordSHA512 = userPassword.SHA512(),
                Totp = totp
            });

            if (response is null || response.Auth.NullOrEmpty() || response.PrivateKey.NullOrEmpty())
            {
                return 2;
            }

            try
            {
                user.Id = appSettings.LastUserId = userId;
                appSettings.Save();

                user.PublicKeyPem = KeyExchangeUtility.DecompressPublicKey(response.PublicKey);
                user.PrivateKeyPem = KeyExchangeUtility.DecompressAndDecryptPrivateKey(response.PrivateKey, userPassword);
                user.Token = new Tuple<DateTime, string>(DateTime.UtcNow, response.Auth);

                return 0;
            }
            catch (Exception)
            {
                return 3;
            }
        }
    }
}