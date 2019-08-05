#region
using System;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for changing an existing <see cref="Convo"/>'s metadata
    /// (such as for example updating the title or description, or extending its lifespan).
    /// </summary>
    public class ConvoChangeMetadataRequestDto : IEquatable<ConvoChangeMetadataRequestDto>
    {
        /// <summary>
        /// The <see cref="User.Id"/> of who is making the request.
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The requesting user's authentication token.
        /// </summary>
        [JsonProperty(PropertyName = "auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The unique id of the <see cref="Convo"/> whose metadata should be changed.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ConvoId { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s access password (hashed using SHA512).
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The new convo admin.
        /// </summary>
        [JsonProperty(PropertyName = "creatorId")]
        public string CreatorId { get; set; }

        /// <summary>
        /// <see cref="Convo"/> name/title.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s new description text.
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The changed access password hash for this <see cref="Convo"/>.
        /// </summary>
        [JsonProperty(PropertyName = "newPw")]
        public string NewConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The new convo expiration <see cref="DateTime"/> (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime? ExpirationUTC { get; set; }

        #region Equality
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ConvoChangeMetadataRequestDto other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(UserId, other.UserId) && string.Equals(Auth, other.Auth) && string.Equals(ConvoId, other.ConvoId) && string.Equals(ConvoPasswordSHA512, other.ConvoPasswordSHA512) && string.Equals(CreatorId, other.CreatorId) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && string.Equals(NewConvoPasswordSHA512, other.NewConvoPasswordSHA512) && ExpirationUTC.Equals(other.ExpirationUTC);
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
            return Equals((ConvoChangeMetadataRequestDto)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Auth != null ? Auth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ConvoId != null ? ConvoId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ConvoPasswordSHA512 != null ? ConvoPasswordSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CreatorId != null ? CreatorId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NewConvoPasswordSHA512 != null ? NewConvoPasswordSHA512.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ExpirationUTC.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}
