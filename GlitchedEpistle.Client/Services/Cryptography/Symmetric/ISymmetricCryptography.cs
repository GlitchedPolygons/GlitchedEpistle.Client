using System.Security.Cryptography;

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
        /// <returns>Decrypted <c>byte[]</c> or <c>null</c> if decryption failed.</returns>
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
        /// <param name="encryptedBytes">The encrypted data.</param>
        /// <param name="password">The password that was used to encrypt the data.</param>
        /// <returns>The decrypted <c>byte[]</c> array.</returns>
        byte[] DecryptWithPassword(byte[] encryptedBytes, string password);

        /// <summary>
        /// Symmetrically encrypts an RSA key using a password and automatically returns the encoded base-64 <see cref="System.String"/>.<para> </para>
        /// To decrypt again, use the <see cref="DecryptRSAParameters"/> method.
        /// </summary>
        /// <param name="key">The RSA key to encrypt.</param>
        /// <param name="password">Password to use to encrypt the key.</param>
        /// <returns>The encrypted, encoded base-64 <see cref="System.String"/>, ready to be exchanged (or decrypted using <see cref="DecryptRSAParameters"/>).</returns>
        string EncryptRSAParameters(RSAParameters key, string password);

        /// <summary>
        /// Symmetrically decrypts an RSA key that was encrypted using the <see cref="EncryptRSAParameters"/> method.
        /// </summary>
        /// <param name="encryptedKey">The encrypted key string.</param>
        /// <param name="password">The password with which the key was encrypted.</param>
        /// <returns>The decrypted <see cref="RSAParameters"/> key instance.</returns>
        RSAParameters DecryptRSAParameters(string encryptedKey, string password);
    }
}
