﻿#region
using System.Security.Cryptography;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// Service interface for encrypting and decrypting <see langword="string"/>s and <see langword="byte"/>[] arrays.
    /// </summary>
    public interface IAsymmetricCryptographyRSA
    {
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="publicKey">The public key for encryption.</param>
        /// <returns>The encrypted <c>string</c>.</returns>
        string Encrypt(string text, RSAParameters publicKey);

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKey">The private RSA key needed for decryption.</param>
        /// <returns>Decrypted <c>string</c></returns>
        /// <exception cref="CryptographicException">If only the public key is provided, the <c>string</c> cannot be decrypted.</exception>
        string Decrypt(string encryptedText, RSAParameters privateKey);

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key.
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c>) to encrypt.</param>
        /// <param name="publicKey">The public key to use for encryption.</param>
        /// <returns>The encrypted bytes (System.Byte[]).</returns>
        byte[] Encrypt(byte[] data, RSAParameters publicKey);

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key.
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKey">The private key to use for decryption.</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful.</returns>
        /// <exception cref="CryptographicException">If only the public key is provided, the <c>byte[]</c> array cannot be decrypted.</exception>
        byte[] Decrypt(byte[] encryptedData, RSAParameters privateKey);
    }
}
