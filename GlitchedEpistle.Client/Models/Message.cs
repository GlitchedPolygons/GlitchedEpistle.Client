using System;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;
using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// An epistle message.
    /// </summary>
    public class Message : IEquatable<Message>
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

        private string id = null;
        /// <summary>
        /// Gets the message's unique identifier, which is <para> </para>
        /// md5( <see cref="SenderId"/> + <see cref="Timestamp"/> )
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = (SenderId + Timestamp.ToString("u")).MD5();
                }
                return id;
            }
        }

        #region Equality

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Message);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Message other)
        {
            return other != null && other.Id == Id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
