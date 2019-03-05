using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Data-transfer object for the creation of a new <see cref="Convo"/>.
    /// </summary>
    public class ConvoCreationDto
    {
        /// <summary>
        /// <see cref="Convo"/> name (title).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <see cref="Convo"/> description (e.g. what is the convo about?).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The convo's access password SHA512.
        /// </summary>
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The conversation's expiration date (in UTC).
        /// </summary>
        public DateTime Expires { get; set; } = DateTime.MaxValue;
    }
}
