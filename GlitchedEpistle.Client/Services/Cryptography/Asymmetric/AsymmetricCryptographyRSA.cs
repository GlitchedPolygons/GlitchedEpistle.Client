using System;
using System.Text;
using System.Security.Cryptography;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// <see cref="IAsymmetricCryptographyRSA"/> implementation for asymmetric RSA encryption/decryption.
    /// </summary>
    /// <seealso cref="IAsymmetricCryptographyRSA" />
    public class AsymmetricCryptographyRSA : IAsymmetricCryptographyRSA
    {
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="publicKey">The public key for encryption.</param>
        /// <returns>The encrypted <c>string</c>.</returns>
        public string Encrypt(string text, RSAParameters publicKey)
        {
            if (string.IsNullOrEmpty(text)) return null;

            byte[] encryptedData;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);
            }
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKey">The private RSA key needed for decryption.</param>
        /// <returns>Decrypted <c>string</c></returns>
        /// <exception cref="CryptographicException"></exception>
        public string Decrypt(string encryptedText, RSAParameters privateKey)
        {
            if (string.IsNullOrEmpty(encryptedText)) return null;

            byte[] data;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                if (rsa.PublicOnly)
                {
                    throw new CryptographicException($"{nameof(AsymmetricCryptographyRSA)}::{nameof(Decrypt)}: You've provided a public key instead of a private key... for decryption you need the private key!");
                }
                data = rsa.Decrypt(Convert.FromBase64String(encryptedText), true);
            }
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key.
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c>) to encrypt.</param>
        /// <param name="publicKey">The public key to use for encryption.</param>
        /// <returns>The encrypted bytes (System.Byte[]).</returns>
        public byte[] Encrypt(byte[] data, RSAParameters publicKey)
        {
            byte[] encryptedData;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                encryptedData = rsa.Encrypt(data, true);
            }
            return encryptedData;
        }

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key.
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKey">The private key to use for decryption.</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful.</returns>
        /// <exception cref="CryptographicException"></exception>
        public byte[] Decrypt(byte[] encryptedData, RSAParameters privateKey)
        {
            byte[] data;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                if (rsa.PublicOnly)
                {
                    throw new CryptographicException($"{nameof(AsymmetricCryptographyRSA)}::{nameof(Decrypt)}: You've provided a public key instead of a private key... for decryption you need the private key!");
                }
                data = rsa.Decrypt(encryptedData, true);
            }
            return data;
        }
    }
}
