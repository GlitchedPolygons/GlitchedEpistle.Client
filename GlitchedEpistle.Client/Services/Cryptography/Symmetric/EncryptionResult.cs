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
        public byte[] IV { get; set; }

        /// <summary>
        /// The encryption key.
        /// </summary>
        public byte[] Key { get; set; }

        /// <summary>
        /// The encrypted data bytes.
        /// </summary>
        public byte[] EncryptedData { get; set; }

        /// <summary>
        /// Overwrites the <see cref="Key"/> and <see cref="IV"/> bytes with zeros.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < IV.Length; i++) IV[i] = 0;
            for (int i = 0; i < Key.Length; i++) Key[i] = 0;
        }
    }
}
