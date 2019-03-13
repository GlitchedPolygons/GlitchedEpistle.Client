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
        
        public bool Equals(UserCreationDto other)
        {
            return string.Equals(PasswordHash, other.PasswordHash) &&
                   string.Equals(PublicKeyXml, other.PublicKeyXml) &&
                   string.Equals(CreationSecret, other.CreationSecret);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserCreationDto other && Equals(other);
        }

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