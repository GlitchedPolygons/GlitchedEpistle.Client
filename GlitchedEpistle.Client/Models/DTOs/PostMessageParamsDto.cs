using System;
using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for the message post request parameters (request body).
    /// </summary>
    public class PostMessageParamsDto
    {
        /// <summary>
        /// The message's unique id.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The conversation's access pw.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The message author's user id.
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The message author's request authentication token.
        /// </summary>
        [JsonProperty(PropertyName = "auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The message author's username (to display).
        /// </summary>
        [JsonProperty(PropertyName = "senderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// The message's encrypted bodies (packed into json key-value pairs).
        /// </summary>
        [JsonProperty(PropertyName = "bodies")]
        public string MessageBodiesJson { get; set; }

        /// <summary>
        /// The message's timestamp (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "utc")]
        public DateTime TimestampUTC { get; set; }
    }
}
