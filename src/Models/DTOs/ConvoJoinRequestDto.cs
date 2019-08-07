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
