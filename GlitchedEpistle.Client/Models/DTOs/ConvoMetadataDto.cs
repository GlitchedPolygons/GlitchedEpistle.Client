using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Data transfer object class for a conversation's metadata (like e.g. the name, description, etc...).
    /// Does not contain sensitive information such as passwords, etc...
    /// </summary>
    public class ConvoMetadataDto
    {
        /// <summary>
        /// Unique <see cref="Convo"/> identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The creator user identifier.
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// <see cref="Convo"/> name/title.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s description text.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The <see cref="Convo"/>'s creation timestamp (UTC).
        /// </summary>
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// Convo expiration <see cref="DateTime"/> (UTC).
        /// </summary>
        public DateTime ExpirationUTC { get; set; }

        /// <summary>
        /// The convo's participants (user ids separated by a comma: ',').
        /// </summary>
        /// <value>The participants.</value>
        public string Participants { get; set; }

        /// <summary>
        /// The convo's banned users (their user id), separated by a comma: ','.
        /// </summary>
        /// <value>The banned users.</value>
        public string BannedUsers { get; set; }
    }
}
