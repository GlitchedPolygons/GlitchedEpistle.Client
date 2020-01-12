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

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Convo join request DTO containing <see cref="Convo"/> credentials.
    /// </summary>
    public class ConvoJoinRequestDto
    {
        /// <summary>
        /// The <see cref="Convo"/>'s unique backend ID.
        /// </summary>
        [JsonProperty("id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s password, hashed using SHA512.
        /// </summary>
        [JsonProperty("pw")]
        public string ConvoPasswordSHA512 { get; set; }
    }
}
