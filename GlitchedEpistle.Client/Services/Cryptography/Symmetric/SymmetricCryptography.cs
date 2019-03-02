using System.Security.Cryptography;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    /// <summary>
    /// Service interface implementation for symmetrically encrypting/decrypting data (raw <c>byte[]</c> arrays) using <see cref="AesManaged"/>.
    /// Implements the <see cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric.ISymmetricCryptography" /> <see langword="interface"/>.
    /// </summary>
    /// <seealso cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric.ISymmetricCryptography" />
    public class SymmetricCryptography : ISymmetricCryptography
    {
        /// <summary>
        /// Encrypts the specified data using a randomly generated key and initialization vector.<para></para>
        /// Returns an <see cref="EncryptionResult" /> containing the encrypted <c>byte[]</c> array + the used encryption key and iv.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns><see cref="EncryptionResult" /> containing the encrypted <c>byte[]</c> array + the used encryption key and iv.</returns>
        public EncryptionResult Encrypt(byte[] data)
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
                        iv = aes.IV,
                        key = aes.Key,
                        encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length)
                    };
                }
            }
            return result;
        }

        /// <summary>
        /// Decrypts the specified <see cref="EncryptionResult" /> that was obtained using <see cref="Encrypt(System.Byte[])" />.
        /// </summary>
        /// <param name="encryptionResult">The <see cref="EncryptionResult" /> that was obtained using <see cref="Encrypt(System.Byte[])" />.</param>
        /// <returns>Decrypted <c>byte[]</c> or <see langword="null" /> if decryption failed.</returns>
        public byte[] Decrypt(EncryptionResult encryptionResult)
        {
            byte[] decryptedBytes;
            using (var aes = new AesManaged())
            {
                aes.IV = encryptionResult.iv;
                aes.Key = encryptionResult.key;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    decryptedBytes = decryptor.TransformFinalBlock(encryptionResult.encryptedData, 0, encryptionResult.encryptedData.Length);
                }
            }
            return decryptedBytes;
        }
    }
}
