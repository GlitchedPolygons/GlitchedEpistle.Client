using System;
using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// An epistle message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The sender's ID.
        /// </summary>
        [JsonProperty(PropertyName = "senderId")]
        public string SenderId { get; set; }

        /// <summary>
        /// The sender's display username.
        /// </summary>
        [JsonProperty(PropertyName = "senderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// The message's timestamp in UTC.
        /// </summary>
        [JsonProperty(PropertyName = "utc")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// This is the message body - a json string that's been encrypted specifically for its recipient user (using that user's public RSA key).
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
    }
}
