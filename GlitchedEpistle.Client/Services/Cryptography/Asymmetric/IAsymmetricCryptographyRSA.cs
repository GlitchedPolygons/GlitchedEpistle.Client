#region
using System.Security.Cryptography;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// Service interface for encrypting and decrypting <c>string</c>s and <c>byte</c>[] arrays.<para> </para>
    /// Please note that these methods can and WILL throw exceptions if the cryptographic operations fail for some reason.<para> </para>
    /// Make sure that everything you encrypt with these methods will also be decrypted again using the same code.<para> </para>
    /// DO NOT MIX crypto libraries under any circumstances!
    /// </summary>
    public interface IAsymmetricCryptographyRSA
    {
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="text">The plain text to encrypt.</param>
        /// <param name="publicKeyPem">The public RSA key for encryption (PEM-formatted <c>string</c>).</param>
        /// <returns>The encrypted <c>string</c>; <c>null</c> if the passed key or plain text argument was <c>null</c> or empty.</returns>
        string Encrypt(string text, string publicKeyPem);

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKeyPem">The private RSA key needed for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted <c>string</c>; <c>null</c> if the passed key or encrypted text argument was <c>null</c> or empty.</returns>
        string Decrypt(string encryptedText, string privateKeyPem);

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>..
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c> array) to encrypt.</param>
        /// <param name="publicKeyPem">The public key (PEM-formatted <c>string</c>) to use for encryption.</param>
        /// <returns>The encrypted bytes (<c>System.Byte[]</c>); <c>null</c> if the passed data or key argument was <c>null</c> or empty.</returns>
        byte[] Encrypt(byte[] data, string publicKeyPem);

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKeyPem">The private RSA key to use for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful; <c>null</c> if the passed data or key argument was <c>null</c> or empty.</returns>
        byte[] Decrypt(byte[] encryptedData, string privateKeyPem);
    }
}
