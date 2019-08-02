#region
using System;
using System.Collections.Generic;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// A <c>class</c> containing the HTTP response data for <see cref="User"/> registration.
    /// </summary>
    public class UserCreationResponseDto : IEquatable<UserCreationResponseDto>
    {
        /// <summary>
        /// The user's unique identifier (the primary key for the epistle db).
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The user's password hashed with SHA512.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The <see cref="DateTime"/> when this <see cref="User"/> was first created.
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// The user's role. 
        /// </summary>
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        /// <summary>
        /// The user's 2FA TOTP secret.
        /// </summary>
        /// <value>The totp secret.</value>
        [JsonProperty(PropertyName = "totps")]
        public string TotpSecret { get; set; }

        /// <summary>
        /// The user's 2FA TOTP emergency backup codes (can only be used once).
        /// </summary>
        /// <value>The 2FA TOTP emergency backup codes.</value>
        [JsonProperty(PropertyName = "sos")]
        public List<string> TotpEmergencyBackupCodes { get; set; }

        /// <summary>
        /// The exact <see cref="DateTime"/> (UTC) this user's access to Epistle expires.
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(UserCreationResponseDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Id, other.Id) && string.Equals(PasswordSHA512, other.PasswordSHA512) && string.Equals(Role, other.Role) && string.Equals(TotpSecret, other.TotpSecret);
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
            return Equals((UserCreationResponseDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PasswordSHA512 != null ? PasswordSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Role != null ? Role.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TotpSecret != null ? TotpSecret.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
