using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for creating a new <see cref="User"/>.
    /// </summary>
    public class UserCreationDto : IEquatable<UserCreationDto>
    {
        /// <summary>
        /// The user's desired password SHA512.
        /// </summary>
        public string PasswordHash { get; set; }
        
        /// <summary>
        /// The user's public RSA key (in xml format). This is needed to encrypt messages for this user.
        /// </summary>
        public string PublicKeyXml{ get; set; }
        
        /// <summary>
        /// The server creation secret <c>string</c>.
        /// </summary>
        public string CreationSecret{ get; set; }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(UserCreationDto other)
        {
            return other != null && 
                   string.Equals(PasswordHash, other.PasswordHash) &&
                   string.Equals(PublicKeyXml, other.PublicKeyXml) &&
                   string.Equals(CreationSecret, other.CreationSecret);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserCreationDto other && Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = PasswordHash.GetHashCode();
                hashCode = (hashCode * 397) ^ PublicKeyXml.GetHashCode();
                hashCode = (hashCode * 397) ^ CreationSecret.GetHashCode();
                return hashCode;
            }
        }
        
        #endregion
    }
}