using System;
using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Data-transfer object for the creation of a new <see cref="Convo"/>.
    /// </summary>
    public class ConvoCreationDto
    {
        /// <summary>
        /// <see cref="Convo"/> name (title).
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// <see cref="Convo"/> description (e.g. what is the convo about?).
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The convo's access password SHA512.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The conversation's expiration date (in UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; } = DateTime.MaxValue;
    }
}
