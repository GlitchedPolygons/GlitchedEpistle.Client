using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for convo deletion requests.
    /// </summary>
    public class ConvoDeletionRequestDto
    {
        /// <summary>
        /// The id of the <see cref="Convo"/> to delete.
        /// </summary>
        [JsonProperty("convoId")]
        public string ConvoId { get; set; }

        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }
    }
}
