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

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// <see cref="IAsymmetricCryptographyRSA"/> implementation for asymmetric RSA encryption/decryption.
    /// </summary>
    /// <seealso cref="IAsymmetricCryptographyRSA" />
    public class AsymmetricCryptographyRSA : IAsymmetricCryptographyRSA
    {
        private static AsymmetricCipherKeyPair PemStringToKeyPair(string rsaKeyPem)
        {
            using (var stringReader = new StringReader(rsaKeyPem))
            {
                var pemReader = new PemReader(stringReader);
                return pemReader.ReadObject() as AsymmetricCipherKeyPair;
            }
        }
        
        /// <summary>
        /// Encrypts or decrypts the input <paramref name="data"/> parameter
        /// according to the <paramref name="encrypt"/> <c>bool</c> parameter, using the provided RSA <paramref name="key"/>.<para> </para>
        /// If <paramref name="encrypt"/> is set to <c>false</c>, the method will try to decrypt instead.<para> </para>
        /// This method can throw exceptions! E.g. don't pass any <c>null</c> or invalid arguments.
        /// Trying to decrypt with a <c>null</c> or public <paramref name="key"/> will throw exceptions! Make sure to wrap the call to this method in a try/catch block.
        /// </summary>
        /// <param name="data">The data to encrypt or decrypt</param>
        /// <param name="key">The RSA key to use for encryption/decryption.</param>
        /// <param name="encrypt">Should the method encrypt the passed input <paramref name="data"/> or attempt to decrypt it?</param>
        /// <returns>The processed data <c>byte[]</c> array; exceptions are thrown in case of a failure.</returns>
        private static byte[] ProcessData(byte[] data, ICipherParameters key, bool encrypt)
        {
            // PKCS1 OAEP paddings
            OaepEncoding eng = new OaepEncoding(new RsaEngine());
            eng.Init(encrypt, key);

            int length = data.Length;
            int blockSize = eng.GetInputBlockSize();

            List<byte> processedBytes = new List<byte>(length);

            for (int chunkPosition = 0; chunkPosition < length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                processedBytes.AddRange(eng.ProcessBlock(data, chunkPosition, chunkSize));
            }

            return processedBytes.ToArray();
        }
        
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="text">The plain text to encrypt.</param>
        /// <param name="publicKeyPem">The public RSA key for encryption (PEM-formatted <c>string</c>).</param>
        /// <returns>The encrypted <c>string</c>; <c>string.Empty</c> if the passed key or plain text argument was <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        public string Encrypt(string text, string publicKeyPem)
        {
            if (text.NullOrEmpty() || publicKeyPem.NullOrEmpty())
            {
                return string.Empty;
            }
            try
            {
                return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(text), publicKeyPem));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKeyPem">The private RSA key needed for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted <c>string</c>; <c>null</c> if the passed key or encrypted text argument was <c>null</c> or empty; <c>null</c> if decryption failed.</returns>
        public string Decrypt(string encryptedText, string privateKeyPem)
        {
            if (encryptedText.NullOrEmpty() || privateKeyPem.NullOrEmpty())
            {
                return string.Empty;
            }
            try
            {
                return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptedText), privateKeyPem));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c> array) to encrypt.</param>
        /// <param name="publicKeyPem">The public key (PEM-formatted <c>string</c>) to use for encryption.</param>
        /// <returns>The encrypted bytes (<c>System.Byte[]</c>); <c>Array.Empty&lt;byte&gt;()</c> if the passed data or key argument was <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        public byte[] Encrypt(byte[] data, string publicKeyPem)
        {
            if (data is null || data.Length == 0 || publicKeyPem.NullOrEmpty())
            {
                return Array.Empty<byte>();
            }

            try
            {
                ICipherParameters key = null;
                AsymmetricCipherKeyPair keyPair = PemStringToKeyPair(publicKeyPem);
                
                using (var stringReader = new StringReader(publicKeyPem))
                {
                    key = keyPair?.Public ?? new PemReader(stringReader).ReadObject() as RsaKeyParameters;
                }

                return ProcessData(data, key, true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKeyPem">The private RSA key to use for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful; an empty <c>byte[]</c> array if the passed data or key argument was <c>null</c> or empty; <c>null</c> if decryption failed.</returns>
        public byte[] Decrypt(byte[] encryptedData, string privateKeyPem)
        {
            if (encryptedData is null || encryptedData.Length == 0 || privateKeyPem.NullOrEmpty())
            {
                return Array.Empty<byte>();
            }

            try
            {
                return ProcessData(encryptedData, PemStringToKeyPair(privateKeyPem).Private, false);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}