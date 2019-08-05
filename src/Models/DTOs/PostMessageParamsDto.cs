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
            return string.Equals(ConvoPasswordSHA512, other.ConvoPasswordSHA512) && string.Equals(UserId, other.UserId) && string.Equals(Auth, other.Auth) && string.Equals(SenderName, other.SenderName) && string.Equals(MessageBodiesJson, other.MessageBodiesJson);
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
                int hashCode = (ConvoPasswordSHA512 != null ? ConvoPasswordSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Auth != null ? Auth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SenderName != null ? SenderName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MessageBodiesJson != null ? MessageBodiesJson.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
