using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages
{
    /// <summary>
    /// Service interface for comfortably encrypting/decrypting epistle messages (usually json strings).
    /// <seealso cref="IAsymmetricCryptographyRSA"/>
    /// </summary>
    public interface IMessageCryptography
    {
        /// <summary>
        /// Encrypts a message json <c>string</c> for a specific recipient,
        /// whose public encryption RSA key you know (PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json (<c>string</c>) to encrypt.</param>
        /// <param name="recipientPublicRsaKeyPem">The recipient's public RSA key (used for encryption).</param>
        /// <returns>The encrypted message <c>string</c>; <c>string.Empty</c> if the passed parameters were <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        string EncryptMessage(string messageJson, string recipientPublicRsaKeyPem);

        /// <summary>
        /// Decrypts a message that's been encrypted using the <see cref="EncryptMessage(string,string)"/> method.
        /// </summary>
        /// <param name="encryptedMessageJson">The encrypted message <c>string</c> obtained via <see cref="EncryptMessage"/>.</param>
        /// <param name="privateDecryptionRsaKeyPem">Your private message decryption RSA key (PEM-formatted <c>string</c>).</param>
        /// <returns>The decrypted message json; <c>null</c> if decryption failed in some way; an empty <c>string</c> if the passed arguments were <c>null</c> or empty.</returns>
        string DecryptMessage(string encryptedMessageJson, string privateDecryptionRsaKeyPem);
    }
}
