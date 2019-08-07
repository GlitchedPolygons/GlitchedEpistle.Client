using System;
using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for the message post request parameters (request body).
    /// </summary>
    public class PostMessageParamsDto : IEquatable<PostMessageParamsDto>
    {
        /// <summary>
        /// The convo's unique ID.
        /// </summary>
        [JsonProperty("id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The conversation's access pw.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The message author's username (to display).
        /// </summary>
        [JsonProperty(PropertyName = "sndr")]
        public string SenderName { get; set; }

        /// <summary>
        /// The message's encrypted bodies (packed into json key-value pairs).
        /// </summary>
        [JsonProperty(PropertyName = "bodies")]
        public string MessageBodiesJson { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(PostMessageParamsDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(ConvoId, other.ConvoId) && string.Equals(ConvoPasswordSHA512, other.ConvoPasswordSHA512) && string.Equals(SenderName, other.SenderName) && string.Equals(MessageBodiesJson, other.MessageBodiesJson);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((PostMessageParamsDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ConvoId.GetHashCode();
                hashCode = (hashCode * 397) ^ ConvoPasswordSHA512.GetHashCode();
                hashCode = (hashCode * 397) ^ SenderName.GetHashCode();
                hashCode = (hashCode * 397) ^ MessageBodiesJson.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Compares the two objects for equality.
        /// </summary>
        /// <param name="left">Left-hand side of the operator.</param>
        /// <param name="right">Right-hand side of the operator.</param>
        /// <returns>Whether the two objects are equal.</returns>
        public static bool operator ==(PostMessageParamsDto left, PostMessageParamsDto right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Compares the two objects for inequality.
        /// </summary>
        /// <param name="left">Left-hand side of the operator.</param>
        /// <param name="right">Right-hand side of the operator.</param>
        /// <returns>Whether the two objects are not equal.</returns>
        public static bool operator !=(PostMessageParamsDto left, PostMessageParamsDto right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
