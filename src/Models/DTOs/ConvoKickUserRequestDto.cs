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

using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// HTTP PUT request DTO for kicking a <see cref="User"/> out of a <see cref="Convo"/>.
    /// <seealso cref="IConvoService.KickUser"/>
    /// </summary>
    public class ConvoKickUserRequestDto
    {
        /// <summary>
        /// The convo's identifier.
        /// </summary>
        [JsonProperty("id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The convo's password hashed using SHA512.
        /// </summary>
        [JsonProperty("pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// Two-Factor Authentication token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }

        /// <summary>
        /// The user id of who you're kicking out.
        /// </summary>
        [JsonProperty("kickId")]
        public string UserIdToKick { get; set; }

        /// <summary>
        /// If set to <c>true</c>, the kicked user won't be able to rejoin the convo permanently.
        /// </summary>
        [JsonProperty("permaBan")]
        public bool PermaBan { get; set; }
    }
}
