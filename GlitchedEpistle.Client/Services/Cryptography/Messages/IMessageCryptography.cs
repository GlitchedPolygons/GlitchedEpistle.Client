#region
using System.Security.Cryptography;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages
{
    /// <summary>
    /// Service interface for comfortably encrypting/decrypting epistle messages (usually json strings).
    /// </summary>
    public interface IMessageCryptography
    {
        /// <summary>
        /// Encrypts a message json <c>string</c> for a specific recipient, whose public encryption RSA key you know (xml <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json (<c>string</c>).</param>
        /// <param name="recipientPublicRsaKey">The recipient's public RSA key.</param>
        /// <returns>The encrypted message <c>string</c>.</returns>
        string EncryptMessage(string messageJson, RSAParameters recipientPublicRsaKey);

        /// <summary>
        /// Decrypts a message that's been encrypted using the <see cref="EncryptMessage"/> method.
        /// </summary>
        /// <param name="encryptedMessageJson">The encrypted message json <see langword="string"/>.</param>
        /// <param name="privateDecryptionRsaKey">Your private decryption RSA key.</param>
        /// <returns>The decrypted message json (or <see langword="null"/> if decryption failed in some way).</returns>
        string DecryptMessage(string encryptedMessageJson, RSAParameters privateDecryptionRsaKey);
    }
}
