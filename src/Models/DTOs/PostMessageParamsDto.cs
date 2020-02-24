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
    /// DTO for the message post request parameters (request body).
    /// </summary>
    public class PostMessageParamsDto : IEquatable<PostMessageParamsDto>
    {
        /// <summary>
        /// The convo's unique ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The conversation's access pw.
        /// </summary>
        [JsonPropertyName("pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The message author's username (to display).
        /// </summary>
        [JsonPropertyName("sndr")]
        public string SenderName { get; set; }

        /// <summary>
        /// The message's encrypted bodies (packed into json key-value pairs).
        /// </summary>
        [JsonPropertyName("bodies")]
        public string MessageBodiesJson { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(PostMessageParamsDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(ConvoId, other.ConvoId) && string.Equals(ConvoPasswordSHA512, other.ConvoPasswordSHA512) && string.Equals(SenderName, other.SenderName) && string.Equals(MessageBodiesJson, other.MessageBodiesJson);
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
            return Equals((PostMessageParamsDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ConvoId.GetHashCode();
                hashCode = (hashCode * 397) ^ ConvoPasswordSHA512.GetHashCode();
                hashCode = (hashCode * 397) ^ SenderName.GetHashCode();
                hashCode = (hashCode * 397) ^ MessageBodiesJson.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Compares the two objects for equality.
        /// </summary>
        /// <param name="left">Left-hand side of the operator.</param>
        /// <param name="right">Right-hand side of the operator.</param>
        /// <returns>Whether the two objects are equal.</returns>
        public static bool operator ==(PostMessageParamsDto left, PostMessageParamsDto right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Compares the two objects for inequality.
        /// </summary>
        /// <param name="left">Left-hand side of the operator.</param>
        /// <param name="right">Right-hand side of the operator.</param>
        /// <returns>Whether the two objects are not equal.</returns>
        public static bool operator !=(PostMessageParamsDto left, PostMessageParamsDto right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
