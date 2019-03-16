using System;
using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Data transfer object class for a conversation's metadata (like e.g. the name, description, etc...).
    /// Does not contain sensitive information such as passwords, etc...
    /// </summary>
    public class ConvoMetadataDto
    {
        /// <summary>
        /// Unique <see cref="Convo"/> identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The creator user identifier.
        /// </summary>
        [JsonProperty(PropertyName = "creatorId")]
        public string CreatorId { get; set; }

        /// <summary>
        /// <see cref="Convo"/> name/title.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s description text.
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s creation timestamp (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// Convo expiration <see cref="DateTime"/> (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; }

        /// <summary>
        /// The convo's participants (user ids separated by a comma: ',').
        /// </summary>
        /// <value>The participants.</value>
        [JsonProperty(PropertyName = "ppl")]
        public string Participants { get; set; }

        /// <summary>
        /// The convo's banned users (their user id), separated by a comma: ','.
        /// </summary>
        /// <value>The banned users.</value>
        [JsonProperty(PropertyName = "ban")]
        public string BannedUsers { get; set; }
    }
}
