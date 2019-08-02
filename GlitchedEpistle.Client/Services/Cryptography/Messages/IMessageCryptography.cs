namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages
{
    /// <summary>
    /// Service interface for comfortably encrypting/decrypting epistle messages (usually json strings).
    /// </summary>
    public interface IMessageCryptography
    {
        /// <summary>
        /// Encrypts a message json <c>string</c> for a specific recipient,
        /// whose public encryption RSA key you know (PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json <c>string</c> to encrypt.</param>
        /// <param name="recipientPublicRsaKeyPem">The recipient's public RSA key (PEM-formatted <c>string</c>).</param>
        /// <returns>The encrypted message <c>string</c> (or <c>null</c> if encryption failed in some way).</returns>
        string EncryptMessage(string messageJson, string recipientPublicRsaKeyPem);

        /// <summary>
        /// Decrypts a message that's been encrypted using the <see cref="EncryptMessage"/> method.
        /// </summary>
        /// <param name="encryptedMessageJson">The encrypted message json <c>string</c>.</param>
        /// <param name="privateDecryptionRsaKeyPem">Your private decryption RSA key (PEM-formatted <c>string</c>).</param>
        /// <returns>The decrypted message json (or <c>null</c> if decryption failed in some way).</returns>
        string DecryptMessage(string encryptedMessageJson, string privateDecryptionRsaKeyPem);
    }
}
