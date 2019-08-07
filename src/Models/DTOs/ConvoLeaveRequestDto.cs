using Newtonsoft.Json;

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
        [JsonProperty("convoId")]
        public string ConvoId { get; set; }

        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }
    }
}
