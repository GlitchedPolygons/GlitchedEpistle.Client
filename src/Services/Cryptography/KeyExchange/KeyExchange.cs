/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Threading.Tasks;
using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.Services.Cryptography.Asymmetric;
using GlitchedPolygons.Services.Cryptography.Symmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.KeyExchange
{
    /// <inheritdoc />
    public class KeyExchange : IKeyExchange
    {
        private readonly ISymmetricCryptography aes;
        private readonly ICompressionUtility compressionUtility;
        private readonly ICompressionUtilityAsync compressionUtilityAsync;

        /// <summary>
        /// Creates a new key exchange utility instance using the specified <see cref="ISymmetricCryptography"/> and <see cref="ICompressionUtility"/> cryptography/compression providers.
        /// </summary>
        /// <param name="aes">The AES implementation to use for encrypting and decrypting private keys.</param>
        /// <param name="compressionUtility">The compression algo to use for (de)compressing keys.</param>
        /// <param name="compressionUtilityAsync">Asynchronous variant of the compression utility to use.</param>
        public KeyExchange(ISymmetricCryptography aes, ICompressionUtility compressionUtility, ICompressionUtilityAsync compressionUtilityAsync)
        {
            this.aes = aes;
            this.compressionUtility = compressionUtility;
            this.compressionUtilityAsync = compressionUtilityAsync;
        }

        /// <summary>
        /// Encrypts <paramref name="privateKeyPem"/> into a portable <c>string</c>
        /// that is safe to exchange with the backend, AS LONG AS THE USER'S PASSWORD IS NOT COMPROMISED.<para> </para>
        /// THE SERVER NEVER HAS YOUR PRIVATE KEY IN PLAIN TEXT!<para> </para>
        /// The private key is encrypted using the user's password, NOT ITS HASH!
        /// Otherwise the server could decrypt the key, because it does have the user's <see cref="User.PasswordSHA512"/>.<para> </para>
        /// Because of this, it is highly advisable to change user passwords often and use an offline, open-source password manager (such as KeePass).
        /// </summary>
        /// <seealso cref="User"/>
        /// <seealso cref="ISymmetricCryptography"/>
        /// <seealso cref="IAsymmetricCryptographyRSA"/>
        /// <seealso cref="IMessageCryptography"/>
        /// <seealso cref="IUserService"/>
        /// <seealso cref="IConvoService"/>
        /// <param name="privateKeyPem">The user's private RSA key (PEM-formatted <c>string</c>).</param>
        /// <param name="userPassword">The user's password (NOT its SHA512!).</param>
        /// <returns><c>string</c> that contains the encrypted and compressed <paramref name="privateKeyPem"/>.</returns>
        public string EncryptAndCompressPrivateKey(string privateKeyPem, string userPassword)
        {
            return compressionUtility.Compress(aes.EncryptWithPassword(privateKeyPem, userPassword));
        }

        /// <summary>
        /// Asynchronous variant of <see cref="IKeyExchange.EncryptAndCompressPrivateKey"/>.
        /// </summary>
        /// <param name="privateKeyPem">The user's private RSA key (PEM-formatted <c>string</c>).</param>
        /// <param name="userPassword">The user's password (NOT its SHA512!).</param>
        /// <returns><c>string</c> that contains the encrypted and compressed <paramref name="privateKeyPem"/>.</returns>
        public Task<string> EncryptAndCompressPrivateKeyAsync(string privateKeyPem, string userPassword)
        {
            return compressionUtilityAsync.Compress(aes.EncryptWithPassword(privateKeyPem, userPassword));
        }

        /// <summary>
        /// Decompresses and decrypts a private RSA key 
        /// that was encrypted and compressed using <see cref="EncryptAndCompressPrivateKey"/>,
        /// ready to be assigned to <see cref="User.PrivateKeyPem"/>.
        /// </summary>
        /// <param name="encryptedCompressedKey">The encrypted and compressed private key that you'd get from/to the backend (THE SERVER NEVER HAS YOUR PRIVATE KEY IN PLAIN TEXT).</param>
        /// <param name="userPassword">The user's password (NOT the hash).</param>
        /// <returns>The raw PEM-formatted private RSA Key (ready to be assigned to <see cref="User.PrivateKeyPem"/>).</returns>
        public string DecompressAndDecryptPrivateKey(string encryptedCompressedKey, string userPassword)
        {
            return aes.DecryptWithPassword(compressionUtility.Decompress(encryptedCompressedKey), userPassword);
        }

        /// <summary>
        /// Asynchronous variant of <see cref="IKeyExchange.DecompressAndDecryptPrivateKey"/>.
        /// </summary>
        /// <param name="encryptedCompressedKey">The encrypted and compressed private key that you'd get from/to the backend (THE SERVER NEVER HAS YOUR PRIVATE KEY IN PLAIN TEXT).</param>
        /// <param name="userPassword">The user's password (NOT the hash).</param>
        /// <returns>The raw PEM-formatted private RSA Key (ready to be assigned to <see cref="User.PrivateKeyPem"/>).</returns>
        public async Task<string> DecompressAndDecryptPrivateKeyAsync(string encryptedCompressedKey, string userPassword)
        {
            return aes.DecryptWithPassword(await compressionUtilityAsync.Decompress(encryptedCompressedKey), userPassword);
        }

        /// <summary>
        /// Returns the compressed, base-64 encoded <paramref name="publicKeyPem"/>... ready to be exchanged with the backend.
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to compress.</param>
        /// </summary>
        public string CompressPublicKey(string publicKeyPem)
        {
            return compressionUtility.Compress(publicKeyPem);
        }

        /// <summary>
        /// Asynchronous variant of <see cref="IKeyExchange.CompressPublicKey"/>.
        /// </summary>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to compress.</param>
        /// <returns>The compressed key.</returns>
        public Task<string> CompressPublicKeyAsync(string publicKeyPem)
        {
            return compressionUtilityAsync.Compress(publicKeyPem);
        }

        /// <summary>
        /// Decompresses the <paramref name="compressedPublicKeyPem"/> that is
        /// coming from a backend request's response and was initially compressed using <see cref="CompressPublicKey"/>.
        /// </summary>
        /// <param name="compressedPublicKeyPem">The compressed public key pem <c>string</c>.</param>
        /// <returns>The decompressed <paramref name="compressedPublicKeyPem"/>.</returns>
        public string DecompressPublicKey(string compressedPublicKeyPem)
        {
            return compressionUtility.Decompress(compressedPublicKeyPem);
        }

        /// <summary>
        /// Asynchronous variant of <see cref="IKeyExchange.DecompressPublicKey"/>.
        /// </summary>
        /// <param name="compressedPublicKeyPem">The compressed public key pem <c>string</c>.</param>
        /// <returns>The decompressed <paramref name="compressedPublicKeyPem"/>.</returns>
        public Task<string> DecompressPublicKeyAsync(string compressedPublicKeyPem)
        {
            return compressionUtilityAsync.Decompress(compressedPublicKeyPem);
        }
    }
}
