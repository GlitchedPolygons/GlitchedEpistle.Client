namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    /// <summary>
    /// Service interface for symmetrically encrypting/decrypting data (raw <c>byte[]</c> arrays).<para> </para>
    /// Please keep in mind that the data you encrypt with <see cref="EncryptWithPassword(byte[],string)"/> can only be decrypted using the same password and the corresponding mirror method <see cref="DecryptWithPassword(byte[],string)"/>.<para> </para>
    /// Likewise, data encrypted using <see cref="Encrypt"/> can only be decrypted again using <see cref="Decrypt"/> respectively.
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
        /// Decrypts the specified <see cref="EncryptionResult"/> that was obtained using <see cref="ISymmetricCryptography.Encrypt(byte[])"/>.
        /// </summary>
        /// <param name="encryptionResult">The <see cref="EncryptionResult"/> that was obtained using <see cref="ISymmetricCryptography.Encrypt(byte[])"/>.</param>
        /// <returns>Decrypted <c>byte[]</c> or <see langword="null"/> if decryption failed.</returns>
        byte[] Decrypt(EncryptionResult encryptionResult);

        /// <summary>
        /// Encrypts data using a password.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="password">The password used to derive the AES key.</param>
        /// <returns>The encrypted data bytes.</returns>
        byte[] EncryptWithPassword(byte[] data, string password);

        /// <summary>
        /// Decrypts data that was encrypted using <see cref="EncryptWithPassword"/>.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="password">The password that was used to encrypt the data.</param>
        /// <returns>The decrypted <c>byte[]</c> array.</returns>
        byte[] DecryptWithPassword(byte[] encryptedData, string password);
    }
}
