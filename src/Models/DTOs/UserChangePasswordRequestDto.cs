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
            return string.Equals(OldPwSHA512, other.OldPwSHA512) && string.Equals(NewPwSHA512, other.NewPwSHA512) && string.Equals(NewPrivateKey, other.NewPrivateKey);
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
                int hashCode = OldPwSHA512.GetHashCode();
                hashCode = (hashCode * 397) ^ NewPwSHA512.GetHashCode();
                hashCode = (hashCode * 397) ^ NewPrivateKey.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}
