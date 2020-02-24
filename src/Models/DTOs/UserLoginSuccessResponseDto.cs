/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The user's public key.
        /// Others need this in order to send the <see cref="User"/> messages.
        /// </summary>
        [JsonPropertyName("key")]
        public string PublicKey { get; set; }

        /// <summary>
        /// The user's private key; PEM-formatted and encrypted into <c>byte[]</c> and then compressed and base-64 encoded.
        /// </summary>
        [JsonPropertyName("pkey")]
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
