#region
using System;
using System.Text;
using System.Collections.Generic;

using GlitchedPolygons.RepositoryPattern;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Users;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// A highly civilized conversation between two or more homo sapiens.
    /// </summary>
    public class Convo : IEquatable<ConvoMetadataDto>, IEquatable<Convo>, IEntity<string>
    {
        /// <summary>
        /// Unique identifier for the convo.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// User ID of the conversation's creator.
        /// </summary>
        [JsonProperty(PropertyName = "creatorId")]
        public string CreatorId { get; set; }

        /// <summary>
        /// The conversation's name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// A short description of what the convo is about.
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The convo's password hashed with SHA512.
        /// </summary>
        [JsonIgnore]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The <see cref="DateTime"/> (UTC) this conversation was created.
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// The exact UTC <see cref="DateTime"/> when the convo will expire.<para> </para>
        /// After this moment in time, no further messages can be posted to the convo
        /// and the conversation itself will be deleted 48h afterwards.
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// The people who joined the convo (their user ids).
        /// </summary>
        [JsonProperty(PropertyName = "ppl")]
        public List<string> Participants { get; set; } = new List<string>(2);

        /// <summary>
        /// A list of all the perma-banned users.
        /// </summary>
        [JsonProperty(PropertyName = "ban")]
        public List<string> BannedUsers { get; set; } = new List<string>(2);

        /// <summary>
        /// The conversation's messages.
        /// </summary>
        [JsonIgnore]
        public List<Message> Messages { get; set; } = new List<Message>(16);

        /// <summary>
        /// Determines whether this <see cref="Convo"/> is expired.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="Convo"/> is expired; otherwise, <c>false</c>.</returns>
        public bool IsExpired() => DateTime.UtcNow > ExpirationUTC;

        /// <summary>
        /// Gets all of the <see cref="Convo"/>'s participants (their ids) comma-separated;
        /// ready for submitting them with <see cref="IUserService.GetUserPublicKeyXml"/>.
        /// </summary>
        /// <returns>The participant user ids separated by commas.</returns>
        public string GetParticipantIdsCommaSeparated()
        {
            StringBuilder stringBuilder = new StringBuilder(128);
            int participantsCount = Participants.Count;
            for (int i = 0; i < participantsCount; i++)
            {
                stringBuilder.Append(Participants[i]);
                if (i < participantsCount - 1)
                {
                    stringBuilder.Append(',');
                }
            }
            return stringBuilder.ToString();
        }

        public bool Equals(ConvoMetadataDto other)
        {
            return other != null
                   && Id == other.Id
                   && Name == other.Name
                   && CreatorId == other.CreatorId
                   && Description == other.Description
                   && ExpirationUTC.AlmostEquals(other.ExpirationUTC)
                   && CreationTimestampUTC.AlmostEquals(other.CreationTimestampUTC)
                   && BannedUsers.UnorderedEqual(other.BannedUsers.Split(','))
                   && Participants.UnorderedEqual(other.Participants.Split(','));
        }

        public bool Equals(Convo other)
        {
            return other != null
                   && Id == other.Id
                   && Name == other.Name
                   && CreatorId == other.CreatorId
                   && Description == other.Description
                   && ExpirationUTC.AlmostEquals(other.ExpirationUTC)
                   && CreationTimestampUTC.AlmostEquals(other.CreationTimestampUTC)
                   && BannedUsers.UnorderedEqual(other.BannedUsers)
                   && Participants.UnorderedEqual(other.Participants);
        }
    }
}
