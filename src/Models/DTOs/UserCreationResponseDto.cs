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
using System.Collections.Generic;
using Newtonsoft.Json;

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
        /// The <see cref="DateTime"/> when this <see cref="User"/> was first created.
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationUTC { get; set; }

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
            return string.Equals(Id, other.Id) && string.Equals(TotpSecret, other.TotpSecret);
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
                hashCode = (hashCode * 397) ^ (TotpSecret != null ? TotpSecret.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}
