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
using GlitchedPolygons.Services.Cryptography.Symmetric;
using GlitchedPolygons.Services.Cryptography.Asymmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.KeyExchange
{
    /// <summary>
    /// This key utility was extracted first of all, obviously,
    /// to avoid code duplication and provide a single point of exchange where API consumers can prepare user RSA keys
    /// for submission to the backend and, conversely, deserialize them back into the client's domain model easily.<para> </para>
    /// Secondly, it's also some sort of code entry point for whoever wants to verify Epistle's safety:
    /// you can Shift-F12/navigate into the various message exchange/encryption relevant implementations
    /// from here and convince yourself whether this product is for you or not, by reading the source code.
    /// </summary>
    public interface IKeyExchange
    {
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
        string EncryptAndCompressPrivateKey(string privateKeyPem, string userPassword);
        
        /// <summary>
        /// Asynchronous variant of <see cref="EncryptAndCompressPrivateKey"/>.
        /// </summary>
        /// <param name="privateKeyPem">The user's private RSA key (PEM-formatted <c>string</c>).</param>
        /// <param name="userPassword">The user's password (NOT its SHA512!).</param>
        /// <returns><c>string</c> that contains the encrypted and compressed <paramref name="privateKeyPem"/>.</returns>
        Task<string> EncryptAndCompressPrivateKeyAsync(string privateKeyPem, string userPassword);

        /// <summary>
        /// Decompresses and decrypts a private RSA key 
        /// that was encrypted and compressed using <see cref="EncryptAndCompressPrivateKey"/>,
        /// ready to be assigned to <see cref="User.PrivateKeyPem"/>.
        /// </summary>
        /// <param name="encryptedCompressedKey">The encrypted and compressed private key that you'd get from/to the backend (THE SERVER NEVER HAS YOUR PRIVATE KEY IN PLAIN TEXT).</param>
        /// <param name="userPassword">The user's password (NOT the hash).</param>
        /// <returns>The raw PEM-formatted private RSA Key (ready to be assigned to <see cref="User.PrivateKeyPem"/>).</returns>
        string DecompressAndDecryptPrivateKey(string encryptedCompressedKey, string userPassword);

        /// <summary>
        /// Asynchronous variant of <see cref="DecompressAndDecryptPrivateKey"/>.
        /// </summary>
        /// <param name="encryptedCompressedKey">The encrypted and compressed private key that you'd get from/to the backend (THE SERVER NEVER HAS YOUR PRIVATE KEY IN PLAIN TEXT).</param>
        /// <param name="userPassword">The user's password (NOT the hash).</param>
        /// <returns>The raw PEM-formatted private RSA Key (ready to be assigned to <see cref="User.PrivateKeyPem"/>).</returns>
        Task<string> DecompressAndDecryptPrivateKeyAsync(string encryptedCompressedKey, string userPassword);
        
        /// <summary>
        /// Returns the compressed, base-64 encoded <paramref name="publicKeyPem"/>... ready to be exchanged with the backend.
        /// </summary>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to compress.</param>
        /// <returns>The compressed key.</returns>
        string CompressPublicKey(string publicKeyPem);

        /// <summary>
        /// Asynchronous variant of <see cref="CompressPublicKey"/>.
        /// </summary>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to compress.</param>
        /// <returns>The compressed key.</returns>
        Task<string> CompressPublicKeyAsync(string publicKeyPem);
        
        /// <summary>
        /// Decompresses the <paramref name="compressedPublicKeyPem"/> that is
        /// coming from a backend request's response and was initially compressed using <see cref="CompressPublicKey"/>.
        /// </summary>
        /// <param name="compressedPublicKeyPem">The compressed public key pem <c>string</c>.</param>
        /// <returns>The decompressed <paramref name="compressedPublicKeyPem"/>.</returns>
        string DecompressPublicKey(string compressedPublicKeyPem);
        
        /// <summary>
        /// Asynchronous variant of <see cref="DecompressPublicKey"/>.
        /// </summary>
        /// <param name="compressedPublicKeyPem">The compressed public key pem <c>string</c>.</param>
        /// <returns>The decompressed <paramref name="compressedPublicKeyPem"/>.</returns>
        Task<string> DecompressPublicKeyAsync(string compressedPublicKeyPem);
    }
}
