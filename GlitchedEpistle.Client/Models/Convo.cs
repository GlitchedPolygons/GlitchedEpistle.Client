using System;
using System.Collections.Generic;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// A highly civilized conversation between two or more homo sapiens.
    /// </summary>
    public class Convo
    {
        /// <summary>
        /// Unique identifier for the convo.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User ID of the conversation's creator.
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// The conversation's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A short description of what the convo is about.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The convo's password hashed with SHA512.
        /// </summary>
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The <see cref="DateTime"/> (UTC) this conversation was created.
        /// </summary>
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// The exact UTC <see cref="DateTime"/> when the convo will expire.<para> </para>
        /// After this moment in time, no further messages can be posted to the convo
        /// and the conversation itself will be deleted 48h afterwards.
        /// </summary>
        public DateTime Expires { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// The people who joined the convo (their user ids).
        /// </summary>
        public List<string> Participants { get; set; } = new List<string>(2);

        /// <summary>
        /// A list of all the perma-banned users.
        /// </summary>
        public List<string> BannedUsers { get; set; } = new List<string>(2);

        /// <summary>
        /// The conversation's messages.
        /// </summary>
        public List<Message> Messages { get; set; } = new List<Message>(16);
    }
}
