namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    /// <summary>
    /// Service interface for symmetrically encrypting/decrypting data (raw <c>byte[]</c> arrays).
    /// </summary>
    public interface ISymmetricCryptography
    {
        /// <summary>
        /// Encrypts the specified data using a randomly generated key and initialization vector.<para> </para>
        /// Returns an <see cref="EncryptionResult"/> containing the encrypted <c>byte[]</c> array + the used encryption key and iv.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns><see cref="EncryptionResult"/> containing the encrypted <c>byte[]</c> array + the used encryption key and iv.</returns>
        EncryptionResult Encrypt(byte[] data);

        /// <summary>
        /// Decrypts the specified <see cref="EncryptionResult"/> that was obtained using <see cref="ISymmetricCryptography.Encrypt"/>.
        /// </summary>
        /// <param name="encryptionResult">The <see cref="EncryptionResult"/> that was obtained using <see cref="ISymmetricCryptography.Encrypt"/>.</param>
        /// <returns>Decrypted <c>byte[]</c> or <see langword="null"/> if decryption failed.</returns>
        byte[] Decrypt(EncryptionResult encryptionResult);
    }
}
