﻿/*
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

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// The class that represents the epistle user.
    /// </summary>
    public class User : IEquatable<User>
    {
        /// <summary>
        /// The user's unique identifier (the primary key for the epistle db).
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The user's role. 
        /// </summary>
        [JsonPropertyName("role")]
        public string Role { get; set; } = "User";

        /// <summary>
        /// The user's password hashed with SHA512.
        /// </summary>
        [JsonIgnore]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The token needed to authenticate Web API requests.<para> </para>
        /// <see cref="Tuple{T1,T2}.Item1"/> is the token's UTC timestamp (when it was emitted).<para> </para>
        /// <see cref="Tuple{T1,T2}.Item2"/> is the token <c>string</c>.
        /// </summary>
        [JsonIgnore]
        public Tuple<DateTime, string> Token { get; set; } = null;

        /// <summary>
        /// The <see cref="DateTime"/> when this <see cref="User"/> was first created.
        /// </summary>
        [JsonPropertyName("iat")]
        public long CreationTimestampUTC { get; set; }

        /// <summary>
        /// The user's private message decryption RSA key (PEM-formatted).
        /// </summary>
        [JsonIgnore]
        public string PrivateKeyPem { get; set; }

        /// <summary>
        /// The user's public message encryption RSA key (PEM-formatted).
        /// </summary>
        [JsonIgnore]
        public string PublicKeyPem { get; set; }

        /// <summary>
        /// How many failed login attempts this <see cref="User"/> has on his record.
        /// After too many, he is locked out for a while.
        /// </summary>
        [JsonPropertyName("fail")]
        public int LoginFailures { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Id, other.Id) && string.Equals(Role, other.Role) && string.Equals(PasswordSHA512, other.PasswordSHA512);
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
            return Equals((User)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Role != null ? Role.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PasswordSHA512 != null ? PasswordSHA512.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
