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

using System.Text.Json.Serialization;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for password change requests to the Epistle Web API.
    /// </summary>
    public class UserChangePasswordRequestDto
    {
        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonPropertyName("totp")]
        public string Totp { get; set; }

        /// <summary>
        /// Old password SHA512.
        /// </summary>
        [JsonPropertyName("oldPwSHA512")]
        public string OldPwSHA512 { get; set; }

        /// <summary>
        /// New password's SHA512.
        /// </summary>
        [JsonPropertyName("newPwSHA512")]
        public string NewPwSHA512 { get; set; }

        /// <summary>
        /// New (encrypted) private key.<para> </para>
        /// Needs to be PEM-formatted and encrypted into <c>byte[]</c> and then compressed and base-64 encoded.
        /// </summary>
        [JsonPropertyName("npkey")]
        public string NewPrivateKey { get; set; }
    }
}
