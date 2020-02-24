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
    /// Request parameters for leaving a <see cref="Convo"/>.
    /// </summary>
    public class ConvoLeaveRequestDto
    {
        /// <summary>
        /// The id of the <see cref="Convo"/> to leave.
        /// </summary>
        [JsonPropertyName("convoId")]
        public string ConvoId { get; set; }

        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonPropertyName("totp")]
        public string Totp { get; set; }
    }
}
