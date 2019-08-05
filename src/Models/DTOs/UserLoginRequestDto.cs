using System;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// HTTP request parameter DTO for user login.
    /// </summary>
    public class UserLoginRequestDto : IEquatable<UserLoginRequestDto>
    {
        /// <summary>
        /// The id of the <see cref="User"/> who wants to log in.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The user's password SHA512.
        /// </summary>
        [JsonProperty("pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(UserLoginRequestDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(UserId, other.UserId) && string.Equals(PasswordSHA512, other.PasswordSHA512) && string.Equals(Totp, other.Totp);
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
            return Equals((UserLoginRequestDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PasswordSHA512 != null ? PasswordSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Totp != null ? Totp.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
