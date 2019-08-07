using System;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for password change requests to the Epistle Web API.
    /// </summary>
    public class UserChangePasswordRequestDto : IEquatable<UserChangePasswordRequestDto>
    {
        /// <summary>
        /// The <see cref="User"/> id of who wants to change his password.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Request authentication token.
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// Old password SHA512.
        /// </summary>
        [JsonProperty("oldPwSHA512")]
        public string OldPwSHA512 { get; set; }

        /// <summary>
        /// New password's SHA512.
        /// </summary>
        [JsonProperty("newPwSHA512")]
        public string NewPwSHA512 { get; set; }

        /// <summary>
        /// New (encrypted) private key.<para> </para>
        /// Needs to be PEM-formatted and encrypted into <c>byte[]</c> and then gzipped and base-64 encoded.
        /// </summary>
        [JsonProperty("npkey")]
        public string NewPrivateKey { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(UserChangePasswordRequestDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(UserId, other.UserId) && string.Equals(Auth, other.Auth) && string.Equals(OldPwSHA512, other.OldPwSHA512) && string.Equals(NewPwSHA512, other.NewPwSHA512) && string.Equals(NewPrivateKey, other.NewPrivateKey);
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
            return Equals((UserChangePasswordRequestDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Auth != null ? Auth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OldPwSHA512 != null ? OldPwSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NewPwSHA512 != null ? NewPwSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NewPrivateKey != null ? NewPrivateKey.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
