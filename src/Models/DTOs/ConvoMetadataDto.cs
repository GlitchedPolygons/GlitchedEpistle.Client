﻿#region
using System;
using System.Linq;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Data transfer object class for a conversation's metadata (like e.g. the name, description, etc...).
    /// Does not contain sensitive information such as passwords, etc...
    /// </summary>
    public class ConvoMetadataDto : IEquatable<ConvoMetadataDto>
    {
        /// <summary>
        /// Unique <see cref="Convo"/> identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The creator user identifier.
        /// </summary>
        [JsonProperty(PropertyName = "creatorId")]
        public string CreatorId { get; set; }

        /// <summary>
        /// <see cref="Convo"/> name/title.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s description text.
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s creation timestamp (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// Convo expiration <see cref="DateTime"/> (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; }

        /// <summary>
        /// The convo's participants (user ids separated by a comma: ',').
        /// </summary>
        /// <value>The participants.</value>
        [JsonProperty(PropertyName = "ppl")]
        public string Participants { get; set; }

        /// <summary>
        /// The convo's banned users (their user id), separated by a comma: ','.
        /// </summary>
        /// <value>The banned users.</value>
        [JsonProperty(PropertyName = "ban")]
        public string BannedUsers { get; set; }

        /// <summary>
        /// Converts a <see cref="ConvoMetadataDto"/> data-transfer object from the backend into a full-fletched, client <see cref="Convo"/> instance.
        /// </summary>
        /// <param name="dto"></param>
        public static implicit operator Convo(ConvoMetadataDto dto)
        {
            return new Convo
            {
                Id = dto.Id,
                CreatorId = dto.CreatorId,
                Name = dto.Name,
                Description = dto.Description,
                CreationTimestampUTC = dto.CreationTimestampUTC,
                ExpirationUTC = dto.ExpirationUTC,
                Participants = dto.Participants.Split(',').ToList(),
                BannedUsers = dto.Participants.Split(',').ToList()
            };
        }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ConvoMetadataDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Id, other.Id) &&
                   string.Equals(CreatorId, other.CreatorId) &&
                   string.Equals(Participants, other.Participants) &&
                   string.Equals(BannedUsers, other.BannedUsers);
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
            return obj.GetType() == GetType() && Equals((ConvoMetadataDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ CreatorId.GetHashCode();
                hashCode = (hashCode * 397) ^ Participants.GetHashCode();
                hashCode = (hashCode * 397) ^ BannedUsers.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}