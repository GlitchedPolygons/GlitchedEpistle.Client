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

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for changing an existing <see cref="Convo"/>'s metadata
    /// (such as for example updating the title or description, or extending its lifespan).
    /// </summary>
    public class ConvoChangeMetadataRequestDto
    {
        /// <summary>
        /// The unique id of the <see cref="Convo"/> whose metadata should be changed.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s access password (hashed using SHA512).
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }

        /// <summary>
        /// The new convo admin.
        /// </summary>
        [JsonProperty(PropertyName = "creatorId")]
        public string CreatorId { get; set; }

        /// <summary>
        /// <see cref="Convo"/> name/title.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s new description text.
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The changed access password hash for this <see cref="Convo"/>.
        /// </summary>
        [JsonProperty(PropertyName = "newPw")]
        public string NewConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The new convo expiration <see cref="DateTime"/> (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime? ExpirationUTC { get; set; }
    }
}
