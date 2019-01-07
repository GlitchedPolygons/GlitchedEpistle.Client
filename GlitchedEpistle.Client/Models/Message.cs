using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// An epistle message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The sender's ID.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// The sender's display username.
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The message's timestamp in UTC.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The method's encrypted text.
        /// </summary>
        public string EncryptedText { get; set; }

        /// <summary>
        /// The encrypted attachment file name.
        /// </summary>
        public string EncryptedAttachmentFileName { get; set; }

        /// <summary>
        /// Attached files are not stored inside the message body due to MongoDB's 16MB document size limit.<para> </para>
        /// Instead, a unique <see langword="string"/> is stored for downloading the attachment via GridFS.
        /// </summary>
        public string EncryptedAttachmentFileId { get; set; } = null;
    }
}
