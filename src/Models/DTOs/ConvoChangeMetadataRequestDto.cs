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
