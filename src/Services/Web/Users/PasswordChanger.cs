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
using System.Text.Json;
using System.Threading.Tasks;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.KeyExchange;
using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.Services.Cryptography.Asymmetric;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// <see cref="IPasswordChanger"/> implementation for changing a logged in <see cref="User"/>'s password.
    /// </summary>
    public class PasswordChanger : IPasswordChanger
    {
        private readonly User user;
        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly IKeyExchange keyExchange;
        private readonly IAsymmetricCryptographyRSA crypto;
        private readonly ICompressionUtilityAsync compressionUtility;
        
#pragma warning disable 1591
        public PasswordChanger(User user, IUserService userService, ILogger logger, ICompressionUtilityAsync compressionUtility, IAsymmetricCryptographyRSA crypto, IKeyExchange keyExchange)
        {
            this.user = user;
            this.crypto = crypto;
            this.logger = logger;
            this.keyExchange = keyExchange;
            this.userService = userService;
            this.compressionUtility = compressionUtility;
        }
#pragma warning restore 1591
        
        /// <summary>
        /// Changes an Epistle <see cref="User"/>'s password.
        /// </summary>
        /// <param name="oldPw">The old <see cref="User"/> password (NOT its SHA512!).</param>
        /// <param name="newPw">The new, better and safer password (NOT its SHA512!).</param>
        /// <param name="totp">Request authentication token.</param>
        /// <returns>Whether the password change request was successful or not.</returns>
        /// <exception cref="ApplicationException">Thrown if the <see cref="User.PrivateKeyPem"/> is <c>null</c> or empty.</exception>
        public async Task<bool> ChangePassword(string oldPw, string newPw, string totp)
        {
            if (oldPw == newPw)
            {
                return false;
            }
            
            if (user.PrivateKeyPem.NullOrEmpty())
            {
                string msg = "The user's in-memory private key seems to be null or empty; can't change passwords without re-encrypting a new copy of the user key!";
                logger?.LogError(msg);
                throw new ApplicationException(msg);
            }

            var dto = new UserChangePasswordRequestDto
            {
                Totp = totp,
                OldPwSHA512 = oldPw.SHA512(),
                NewPwSHA512 = newPw.SHA512(),
                NewPrivateKey = await keyExchange.EncryptAndCompressPrivateKeyAsync(user.PrivateKeyPem, newPw).ConfigureAwait(false)
            };

            var requestBody = new EpistleRequestBody
            {
                UserId = user.Id,
                Auth = user.Token.Item2,
                Body = await compressionUtility.Compress(JsonSerializer.Serialize(dto)).ConfigureAwait(false)
            };

            bool success = await userService.ChangeUserPassword(requestBody.Sign(crypto, user.PrivateKeyPem)).ConfigureAwait(false);
            
            return success;
        }
    }
}