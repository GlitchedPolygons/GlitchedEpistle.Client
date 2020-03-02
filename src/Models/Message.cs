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

using GlitchedPolygons.RepositoryPattern;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// An epistle message.
    /// </summary>
    public class Message : IEquatable<Message>, IEntity<long>
    {
        /// <summary>
        /// Gets the message's unique identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// The sender's ID.
        /// </summary>
        [JsonPropertyName("senderId")]
        public string SenderId { get; set; }

        /// <summary>
        /// The sender's display username.
        /// </summary>
        [JsonPropertyName("senderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// The message's timestamp in UTC.
        /// </summary>
        [JsonPropertyName("utc")]
        public long TimestampUTC { get; set; }

        /// <summary>
        /// This is the message body - a json string that's been encrypted specifically for its recipient user (using that user's public RSA key).
        /// </summary>
        [JsonPropertyName("body")]
        public string Body { get; set; }

        /// <summary>
        /// Checks whether the <see cref="Message"/> comes from a <see cref="User"/> or from the Epistle server directly.<para> </para>
        /// Server messages come in the following format: <para> </para>
        /// <c>server:0:f12218f6b9e3481d964a109333f70ae7</c><para> </para>
        /// Message type IDs: <para> </para>
        /// 0 = User joined a convo.
        /// 1 = User left a convo.
        /// 2 = User was kicked out from a convo.
        /// 3 = The convo is about to expire (less than 24h left).
        /// 4 = The convo's metadata was changed.
        /// </summary>
        public bool IsFromServer()
        {
            return SenderId == "0" && SenderName.Equals("Server", StringComparison.InvariantCultureIgnoreCase) && Body.StartsWith("server:", StringComparison.InvariantCultureIgnoreCase);
        }

        #region Equality
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Message);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Message other)
        {
            return other != null 
                   && other.Id == Id 
                   && other.SenderId == SenderId 
                   && other.SenderName == SenderName 
                   && other.Body == Body;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
