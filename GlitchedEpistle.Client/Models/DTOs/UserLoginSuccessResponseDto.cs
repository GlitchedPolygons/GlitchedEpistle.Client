using System;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Response body for successful login requests.
    /// </summary>
    public class UserLoginSuccessResponseDto : IEquatable<UserLoginSuccessResponseDto>
    {
        /// <summary>
        /// Valid request authentication token.
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The user's public key.
        /// Others need this in order to send the <see cref="User"/> messages.
        /// </summary>
        [JsonProperty("key")]
        public string PublicKey { get; set; }

        /// <summary>
        /// The user's private key; PEM-formatted and encrypted into <c>byte[]</c> and then gzipped and base-64 encoded.
        /// </summary>
        [JsonProperty("pkey")]
        public string PrivateKey { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(UserLoginSuccessResponseDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Auth, other.Auth) && string.Equals(PublicKey, other.PublicKey) && string.Equals(PrivateKey, other.PrivateKey);
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
            return Equals((UserLoginSuccessResponseDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Auth != null ? Auth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PublicKey != null ? PublicKey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PrivateKey != null ? PrivateKey.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
