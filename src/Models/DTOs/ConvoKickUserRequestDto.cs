using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// HTTP PUT request DTO for kicking a <see cref="User"/> out of a <see cref="Convo"/>.
    /// <seealso cref="IConvoService.KickUser"/>
    /// </summary>
    public class ConvoKickUserRequestDto
    {
        /// <summary>
        /// The convo's identifier.
        /// </summary>
        [JsonProperty("id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The convo's password hashed using SHA512.
        /// </summary>
        [JsonProperty("pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// Two-Factor Authentication token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }

        /// <summary>
        /// The user id of who you're kicking out.
        /// </summary>
        [JsonProperty("kickId")]
        public string UserIdToKick { get; set; }

        /// <summary>
        /// If set to <c>true</c>, the kicked user won't be able to rejoin the convo permanently.
        /// </summary>
        [JsonProperty("permaBan")]
        public bool PermaBan { get; set; }
    }
}
