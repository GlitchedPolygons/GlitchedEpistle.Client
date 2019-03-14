using System;

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
        public string ConvoPasswordHash { get; set; }

        /// <summary>
        /// The message author's user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The message author's request authentication token.
        /// </summary>
        public string Auth { get; set; }

        /// <summary>
        /// The message author's username (to display).
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The message's encrypted bodies (packed into json key-value pairs).
        /// </summary>
        public string MessageBodiesJson { get; set; }

        /// <summary>
        /// The message's timestamp (UTC).
        /// </summary>
        public DateTime TimestampUTC { get; set; }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(PostMessageParamsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ConvoPasswordHash, other.ConvoPasswordHash, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(UserId, other.UserId, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Auth, other.Auth, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(SenderName, other.SenderName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(MessageBodiesJson, other.MessageBodiesJson, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((PostMessageParamsDto) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (ConvoPasswordHash != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ConvoPasswordHash) : 0);
                hashCode = (hashCode * 397) ^ (UserId != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(UserId) : 0);
                hashCode = (hashCode * 397) ^ (Auth != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Auth) : 0);
                hashCode = (hashCode * 397) ^ (SenderName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(SenderName) : 0);
                hashCode = (hashCode * 397) ^ (MessageBodiesJson != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(MessageBodiesJson) : 0);
                return hashCode;
            }
        }

        #endregion
    }
}