/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

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

using GlitchedPolygons.Services.Cryptography.Asymmetric;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages
{
    /// <summary>
    /// Service interface for comfortably encrypting/decrypting epistle messages (usually json strings).
    /// <seealso cref="IAsymmetricCryptographyRSA"/>
    /// </summary>
    public interface IMessageCryptography
    {
        /// <summary>
        /// Encrypts a message json <c>string</c> for a specific recipient,
        /// whose public encryption RSA key you know (PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json (<c>string</c>) to encrypt.</param>
        /// <param name="recipientPublicRsaKeyPem">The recipient's public RSA key (used for encryption).</param>
        /// <returns>The encrypted message <c>string</c>; <c>string.Empty</c> if the passed parameters were <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        string EncryptMessage(string messageJson, string recipientPublicRsaKeyPem);

        /// <summary>
        /// Decrypts a message that's been encrypted using the <see cref="EncryptMessage(string,string)"/> method.
        /// </summary>
        /// <param name="encryptedMessageJson">The encrypted message <c>string</c> obtained via <see cref="EncryptMessage"/>.</param>
        /// <param name="privateDecryptionRsaKeyPem">Your private message decryption RSA key (PEM-formatted <c>string</c>).</param>
        /// <returns>The decrypted message json; <c>null</c> if decryption failed in some way; an empty <c>string</c> if the passed arguments were <c>null</c> or empty.</returns>
        string DecryptMessage(string encryptedMessageJson, string privateDecryptionRsaKeyPem);
    }
}
