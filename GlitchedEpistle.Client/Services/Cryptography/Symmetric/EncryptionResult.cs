using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    /// <summary>
    /// The result of encrypting some data using the <see cref="SymmetricCryptography.Encrypt"/> method.
    /// Contains the encrypted bytes and the key + initialization vector used for the encryption (you need those for decryption).
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class EncryptionResult : IDisposable
    {
        /// <summary>
        /// The initialization vector.
        /// </summary>
        public byte[] iv;

        /// <summary>
        /// The encryption key.
        /// </summary>
        public byte[] key;

        /// <summary>
        /// The encrypted data bytes.
        /// </summary>
        public byte[] encryptedData;

        /// <summary>
        /// Overwrites the <see cref="key"/> and <see cref="iv"/> bytes with zeros.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < iv.Length; i++) iv[i] = 0;
            for (int i = 0; i < key.Length; i++) key[i] = 0;
        }
    }
}
