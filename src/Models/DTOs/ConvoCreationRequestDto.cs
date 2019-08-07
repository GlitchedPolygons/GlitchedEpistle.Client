#region
using System;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Data-transfer object for the creation of a new <see cref="Convo"/>.
    /// </summary>
    public class ConvoCreationRequestDto : IEquatable<ConvoCreationRequestDto>
    {
        /// <summary>
        /// Two-Factor Authentication token.
        /// </summary>
        [JsonProperty(PropertyName = "totp")]
        public string Totp { get; set; }

        /// <summary>
        /// <see cref="Convo"/> name (title).
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// <see cref="Convo"/> description (e.g. what is the convo about?).
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The convo's access password SHA512.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The conversation's expiration date (in UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; } = DateTime.MaxValue;

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ConvoCreationRequestDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description) &&
                   string.Equals(PasswordSHA512, other.PasswordSHA512);
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
            return obj.GetType() == GetType() && Equals((ConvoCreationRequestDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PasswordSHA512 != null ? PasswordSHA512.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
