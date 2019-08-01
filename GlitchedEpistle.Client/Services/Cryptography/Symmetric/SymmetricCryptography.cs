#region
using System;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Security.Cryptography;

using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    /// <summary>
    /// Service interface implementation for symmetrically encrypting/decrypting data (raw <c>byte[]</c> arrays) using <see cref="AesManaged"/>.<para> </para>
    /// Please keep in mind that the data you encrypt with <see cref="EncryptWithPassword(byte[],string)"/> can only be decrypted using the same password and the corresponding mirror method <see cref="DecryptWithPassword(byte[],string)"/>.<para> </para>
    /// Likewise, data encrypted using <see cref="Encrypt"/> can only be decrypted again using <see cref="Decrypt"/> respectively.
    /// Implements the <see cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric.ISymmetricCryptography" /> <see langword="interface"/>.
    /// </summary>
    public class SymmetricCryptography : ISymmetricCryptography
    {
        private const int RFC_ITERATIONS = 16384;
        private static readonly CompressionSettings KEY_COMPRESSION_SETTINGS = new CompressionSettings { CompressionLevel = CompressionLevel.Fastest };

        /// <summary>
        /// Encrypts the specified data using a randomly generated key and initialization vector.<para></para>
        /// Returns an <see cref="EncryptionResult" /> containing the encrypted <c>byte[]</c> array + the used encryption key and iv.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns><see cref="EncryptionResult" /> containing the encrypted <c>byte[]</c> array + the used encryption key and iv; <c>null</c> if encryption failed.</returns>
        public EncryptionResult Encrypt(byte[] data)
        {
            try
            {
                EncryptionResult result;
                using (var aes = new AesManaged())
                {
                    aes.GenerateIV();
                    aes.GenerateKey();
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        result = new EncryptionResult
                        {
                            IV = aes.IV,
                            Key = aes.Key,
                            EncryptedData = encryptor.TransformFinalBlock(data, 0, data.Length)
                        };
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified <see cref="EncryptionResult" /> that was obtained using <see cref="Encrypt(System.Byte[])" />.
        /// </summary>
        /// <param name="encryptionResult">The <see cref="EncryptionResult" /> that was obtained using <see cref="Encrypt(System.Byte[])" />.</param>
        /// <returns>Decrypted <c>byte[]</c> or <see langword="null" /> if decryption failed.</returns>
        public byte[] Decrypt(EncryptionResult encryptionResult)
        {
            try
            {
                byte[] decryptedBytes;
                using (var aes = new AesManaged())
                {
                    aes.IV = encryptionResult.IV;
                    aes.Key = encryptionResult.Key;
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        decryptedBytes = decryptor.TransformFinalBlock(encryptionResult.EncryptedData, 0, encryptionResult.EncryptedData.Length);
                    }
                }

                return decryptedBytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Encrypts data using a password.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="password">The password used to derive the AES key.</param>
        /// <returns>The encrypted data <c>byte[]</c> array; <c>null</c> if encryption failed in some way.</returns>
        public byte[] EncryptWithPassword(byte[] data, string password)
        {
            try
            {
                byte[] result;
                byte[] salt = new byte[32];
                
                using (var aes = new AesManaged())
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                    
                    using (var rfc = new Rfc2898DeriveBytes(password, salt, RFC_ITERATIONS))
                    {
                        aes.IV = rfc.GetBytes(16);
                        aes.Key = rfc.GetBytes(32);
                        
                        using (ICryptoTransform encryptor = aes.CreateEncryptor())
                        {
                            result = salt.Concat(encryptor.TransformFinalBlock(data, 0, data.Length)).ToArray();
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts data that was encrypted using <see cref="EncryptWithPassword"/>.
        /// </summary>
        /// <param name="encryptedBytes">The encrypted data that was returned by <see cref="EncryptWithPassword(byte[],string)"/>.</param>
        /// <param name="password">The password that was used to encrypt the data.</param>
        /// <returns>The decrypted <c>byte[]</c> array; <c>null</c> if decryption failed.</returns>
        public byte[] DecryptWithPassword(byte[] encryptedBytes, string password)
        {
            try
            {
                byte[] decryptedBytes;
                byte[] salt = new byte[32];
                byte[] encr = new byte[encryptedBytes.Length - 32];

                for (int i = 0; i < salt.Length; i++)
                {
                    salt[i] = encryptedBytes[i];
                }

                for (int i = 0; i < encr.Length; i++)
                {
                    encr[i] = encryptedBytes[i + 32];
                }

                using (var aes = new AesManaged())
                using (var rfc = new Rfc2898DeriveBytes(password, salt, RFC_ITERATIONS))
                {
                    aes.IV = rfc.GetBytes(16);
                    aes.Key = rfc.GetBytes(32);
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        decryptedBytes = decryptor.TransformFinalBlock(encr, 0, encr.Length);
                    }
                }

                return decryptedBytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Symmetrically encrypts an RSA key using a password and automatically returns the encoded base-64 <see cref="System.String"/>.<para> </para>
        /// To decrypt again, use the <see cref="DecryptRSAParameters"/> method.
        /// </summary>
        /// <param name="key">The RSA key to encrypt.</param>
        /// <param name="password">Password to use to encrypt the key.</param>
        /// <returns>The encrypted, encoded base-64 <see cref="System.String"/>, ready to be exchanged (or decrypted using <see cref="DecryptRSAParameters"/>).</returns>
        public string EncryptRSAParameters(RSAParameters key, string password)
        {
            ICompressionUtility gzip = new GZipUtility();

            return Convert.ToBase64String(
                inArray: EncryptWithPassword(
                    password: password,
                    data: gzip.Compress(
                        bytes: Encoding.UTF8.GetBytes(key.ToXmlString()), 
                        compressionSettings: KEY_COMPRESSION_SETTINGS
                    )
                )
            );
        }

        /// <summary>
        /// Symmetrically decrypts an RSA key that was encrypted using the <see cref="EncryptRSAParameters"/> method.
        /// </summary>
        /// <param name="encryptedKey">The encrypted key string.</param>
        /// <param name="password">The password with which the key was encrypted.</param>
        /// <returns>The decrypted <see cref="RSAParameters"/> key instance.</returns>
        public RSAParameters DecryptRSAParameters(string encryptedKey, string password)
        {
            ICompressionUtility gzip = new GZipUtility();

            return RSAParametersExtensions.FromXmlString(
                Encoding.UTF8.GetString(
                    bytes: gzip.Decompress(
                        compressionSettings: KEY_COMPRESSION_SETTINGS,
                        compressedBytes: DecryptWithPassword(
                            password: password,
                            encryptedBytes: Convert.FromBase64String(encryptedKey)
                        )
                    )
                )
            );
        }
    }
}
