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
        #region Encrypting and decrypting
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="text">The plain text to encrypt.</param>
        /// <param name="publicKeyPem">The public RSA key for encryption (PEM-formatted <c>string</c>).</param>
        /// <returns>The encrypted <c>string</c>; <c>string.Empty</c> if the passed key or plain text argument was <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        string Encrypt(string text, string publicKeyPem);

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKeyPem">The private RSA key needed for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted <c>string</c>; <c>null</c> if the passed key or encrypted text argument was <c>null</c> or empty; <c>null</c> if decryption failed.</returns>
        string Decrypt(string encryptedText, string privateKeyPem);

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c> array) to encrypt.</param>
        /// <param name="publicKeyPem">The public key (PEM-formatted <c>string</c>) to use for encryption.</param>
        /// <returns>The encrypted bytes (<c>System.Byte[]</c>); <c>Array.Empty&lt;byte&gt;()</c> if the passed data or key argument was <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        byte[] Encrypt(byte[] data, string publicKeyPem);

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKeyPem">The private RSA key to use for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful; an empty <c>byte[]</c> array if the passed data or key argument was <c>null</c> or empty; <c>null</c> if decryption failed.</returns>
        byte[] Decrypt(byte[] encryptedData, string privateKeyPem);
        #endregion
        
        #region Signing and verifying
        /// <summary>
        /// Signs the specified <c>string</c> using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// If the procedure succeeds, the calculated signature <c>string</c> is returned (which is base-64 encoded). Otherwise,
        /// an empty <c>string</c> is returned if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters
        /// were <c>null</c> or empty. If the procedure fails entirely, <c>null</c> is returned.
        /// </summary>
        /// <param name="data">The data to sign.</param>
        /// <param name="privateKeyPem">The private RSA key to use for generating the signature (PEM-formatted <c>string</c>)</param>
        /// <returns>The signature (base-64 encoded <c>string</c>). <c>string.Empty</c> is returned if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters were <c>null</c> or empty. Returns <c>null</c> if signing failed entirely.</returns>
        string Sign(string data, string privateKeyPem);
        
        /// <summary>
        /// Verifies a signature that was obtained using <see cref="Sign(string,string)"/> with a public RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// </summary>
        /// <param name="data">The data whose signature you want to verify.</param>
        /// <param name="signature">The passed <paramref name="data"/>'s signature (return value of <see cref="Sign(string,string)"/>).</param>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to use for signature verification.</param>
        /// <returns>Whether the data's signature verification succeeded or not.</returns>
        bool Verify(string data, string signature, string publicKeyPem);

        /// <summary>
        /// Signs the specified data <c>byte[]</c> array using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// If the procedure succeeds, the calculated signature <c>byte[]</c> array is returned. Otherwise,
        /// an empty <c>byte[]</c> array is returned if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters
        /// were <c>null</c> or empty. If the procedure fails entirely, <c>null</c> is returned.
        /// </summary>
        /// <param name="data">The data to sign.</param>
        /// <param name="privateKeyPem">The private RSA key to use for generating the signature (PEM-formatted <c>string</c>)</param>
        /// <returns>The signature (<c>byte[]</c>), <c>string.Empty</c> if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters were <c>null</c> or empty. Returns <c>null</c> if signing failed entirely.</returns>
        byte[] Sign(byte[] data, string privateKeyPem);

        /// <summary>
        /// Verifies a signature that was obtained using <see cref="Sign(byte[],string)"/> with a public RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// </summary>
        /// <param name="data">The data whose signature you want to verify.</param>
        /// <param name="signature">The passed <paramref name="data"/>'s signature (return value of <see cref="Sign(byte[],string)"/>).</param>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to use for signature verification.</param>
        /// <returns>Whether the data's signature verification succeeded or not.</returns>
        bool Verify(byte[] data, byte[] signature, string publicKeyPem);
        #endregion
    }
}
