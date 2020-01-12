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

#region
using System;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for creating a new <see cref="User"/>.
    /// </summary>
    public class UserCreationRequestDto : IEquatable<UserCreationRequestDto>
    {
        /// <summary>
        /// The user's desired password SHA512.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The user's public RSA key.<para> </para>
        /// PEM-formatted, and then compressed via <c>Encoding.UTF8.GetBytes(string)</c> using <c>CompressionLevel.Fastest</c> and ultimately base-64 encoded.
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string PublicKey { get; set; }

        /// <summary>
        /// The user's private message decryption RSA key.<para> </para>
        /// PEM-formatted and encrypted into <c>byte[]</c> and then compressed and base-64 encoded.
        /// </summary>
        [JsonProperty("pkey")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// The server creation secret <c>string</c>.
        /// </summary>
        [JsonProperty(PropertyName = "secret")]
        public string CreationSecret { get; set; }

        #region Equality
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(UserCreationRequestDto other)
        {
            return string.Equals(PasswordSHA512, other.PasswordSHA512) &&
                   string.Equals(PublicKey, other.PublicKey) &&
                   string.Equals(CreationSecret, other.CreationSecret);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is UserCreationRequestDto other && Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = PasswordSHA512.GetHashCode();
                hashCode = (hashCode * 397) ^ PublicKey.GetHashCode();
                hashCode = (hashCode * 397) ^ CreationSecret.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}
