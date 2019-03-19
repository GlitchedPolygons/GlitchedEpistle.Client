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
    public class ConvoChangeMetadataDto
    {
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
        [JsonProperty(PropertyName = "pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The new convo expiration <see cref="DateTime"/> (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime? ExpirationUTC { get; set; }
    }
}
