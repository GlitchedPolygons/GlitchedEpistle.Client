#region
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Parameters;

using GlitchedPolygons.GlitchedEpistle.Client.Extensions;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// <see cref="IAsymmetricCryptographyRSA"/> implementation for asymmetric RSA encryption/decryption.
    /// </summary>
    /// <seealso cref="IAsymmetricCryptographyRSA" />
    public class AsymmetricCryptographyRSA : IAsymmetricCryptographyRSA
    {
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="text">The plain text to encrypt.</param>
        /// <param name="publicKeyPem">The public RSA key for encryption (PEM-formatted <c>string</c>).</param>
        /// <returns>The encrypted <c>string</c>; <c>null</c> if the passed key or plain text argument was <c>null</c> or empty.</returns>
        public string Encrypt(string text, string publicKeyPem)
        {
            if (text.NullOrEmpty() || publicKeyPem.NullOrEmpty())
            {
                return null;
            }

            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(text), publicKeyPem));
        }

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKeyPem">The private RSA key needed for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted <c>string</c>; <c>null</c> if the passed key or encrypted text argument was <c>null</c> or empty.</returns>
        public string Decrypt(string encryptedText, string privateKeyPem)
        {
            if (encryptedText.NullOrEmpty() || privateKeyPem.NullOrEmpty())
            {
                return null;
            }

            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptedText), privateKeyPem));
        }

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>..
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c> array) to encrypt.</param>
        /// <param name="publicKeyPem">The public key (PEM-formatted <c>string</c>) to use for encryption.</param>
        /// <returns>The encrypted bytes (<c>System.Byte[]</c>); <c>null</c> if the passed data or key argument was <c>null</c> or empty.</returns>
        public byte[] Encrypt(byte[] data, string publicKeyPem)
        {
            if (data is null || data.Length == 0 || publicKeyPem.NullOrEmpty())
            {
                return null;
            }

            RsaKeyParameters keys;
            using (var stringReader = new StringReader(publicKeyPem))
            {
                var pemReader = new PemReader(stringReader);
                keys = (RsaKeyParameters)pemReader.ReadObject();
            }

            // PKCS1 OAEP paddings
            OaepEncoding eng = new OaepEncoding(new RsaEngine());
            eng.Init(true, keys);

            int length = data.Length;
            int blockSize = eng.GetInputBlockSize();

            List<byte> encryptedBytes = new List<byte>(length);

            for (int chunkPosition = 0; chunkPosition < length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                encryptedBytes.AddRange(eng.ProcessBlock(data, chunkPosition, chunkSize));
            }

            return encryptedBytes.ToArray();
        }

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKeyPem">The private RSA key to use for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful; <c>null</c> if the passed data or key argument was <c>null</c> or empty.</returns>
        public byte[] Decrypt(byte[] encryptedData, string privateKeyPem)
        {
            if (encryptedData is null || encryptedData.Length == 0 || privateKeyPem.NullOrEmpty())
            {
                return null;
            }

            AsymmetricCipherKeyPair keys;
            using (var stringReader = new StringReader(privateKeyPem))
            {
                var pemReader = new PemReader(stringReader);
                keys = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }

            // PKCS1 OAEP paddings
            OaepEncoding eng = new OaepEncoding(new RsaEngine());
            eng.Init(false, keys.Private);

            int length = encryptedData.Length;
            int blockSize = eng.GetInputBlockSize();

            List<byte> decryptedBytes = new List<byte>(length);

            for (int chunkPosition = 0; chunkPosition < length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                decryptedBytes.AddRange(eng.ProcessBlock(encryptedData, chunkPosition, chunkSize));
            }

            return decryptedBytes.ToArray();
        }
    }
}
