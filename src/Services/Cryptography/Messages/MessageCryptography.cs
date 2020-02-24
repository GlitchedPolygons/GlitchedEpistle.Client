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

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.Services.Cryptography.Symmetric;
using GlitchedPolygons.Services.Cryptography.Asymmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages
{
    /// <summary>
    /// Service interface implementation for comfortably encrypting/decrypting epistle messages (usually json strings).
    /// </summary>
    /// <seealso cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages.IMessageCryptography" />
    public class MessageCryptography : IMessageCryptography
    {
        private readonly ILogger logger;
        private readonly ISymmetricCryptography aes;
        private readonly IAsymmetricCryptographyRSA rsa;
        private readonly ICompressionUtility compressionUtility;
        private readonly ICompressionUtilityAsync compressionUtilityAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCryptography"/> class.
        /// </summary>
        /// <param name="aes">The <see cref="ISymmetricCryptography"/> instance (should be injected via IoC).</param>
        /// <param name="rsa">The <see cref="IAsymmetricCryptographyRSA"/> instance (should be injected via IoC).</param>
        /// <param name="compressionUtility">The <see cref="ICompressionUtility"/> instance needed for compression (should be injected via IoC).</param>
        /// <param name="compressionUtilityAsync">The <see cref="ICompressionUtilityAsync"/> instance needed for asynchronous compression (should be injected via IoC).</param>
        /// <param name="logger"><see cref="ILogger"/> instance for logging any cryptographic errors that might occur.</param>
        public MessageCryptography(ISymmetricCryptography aes, IAsymmetricCryptographyRSA rsa, ICompressionUtility compressionUtility, ICompressionUtilityAsync compressionUtilityAsync, ILogger logger)
        {
            this.aes = aes;
            this.rsa = rsa;
            this.logger = logger;
            this.compressionUtility = compressionUtility;
            this.compressionUtilityAsync = compressionUtilityAsync;
        }

        /// <summary>
        /// Encrypts a message json <c>string</c> for a specific recipient,
        /// whose public encryption RSA key you know (PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json (<c>string</c>) to encrypt.</param>
        /// <param name="recipientPublicRsaKeyPem">The recipient's public RSA key (used for encryption).</param>
        /// <returns>The encrypted message <c>string</c>; <c>string.Empty</c> if the passed parameters were <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        public string EncryptMessage(string messageJson, string recipientPublicRsaKeyPem)
        {
            if (messageJson.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(EncryptMessage)}: Message encryption failed; {nameof(messageJson)} string argument was null or empty. Nothing to encrypt!");
                return string.Empty;
            }

            if (recipientPublicRsaKeyPem.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(EncryptMessage)}: Message encryption failed; {nameof(recipientPublicRsaKeyPem)} string argument was null or empty. No key to encrypt with!");
                return string.Empty;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(messageJson);
                using var encryptionResult = aes.Encrypt(data);
                
                var stringBuilder = new StringBuilder(encryptionResult.EncryptedData.Length + 256);
                stringBuilder.Append(Convert.ToBase64String(rsa.Encrypt(encryptionResult.Key, recipientPublicRsaKeyPem)));
                stringBuilder.Append('|');
                stringBuilder.Append(Convert.ToBase64String(encryptionResult.IV));
                stringBuilder.Append('|');
                stringBuilder.Append(Convert.ToBase64String(encryptionResult.EncryptedData));
                
                return compressionUtility.Compress(stringBuilder.ToString());
            }
            catch
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(EncryptMessage)}: Message encryption failed. Eventually an encryption key problem? Passed recipient public RSA key:\r\n{recipientPublicRsaKeyPem}");
                return null;
            }
        }

        /// <summary>
        /// Asynchronously encrypts a message json <c>string</c> for a specific recipient,
        /// whose public encryption RSA key you know (PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json (<c>string</c>) to encrypt.</param>
        /// <param name="recipientPublicRsaKeyPem">The recipient's public RSA key (used for encryption).</param>
        /// <returns>The encrypted message <c>string</c>; <c>string.Empty</c> if the passed parameters were <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        public async Task<string> EncryptMessageAsync(string messageJson, string recipientPublicRsaKeyPem)
        {
            if (messageJson.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(EncryptMessageAsync)}: Message encryption failed; {nameof(messageJson)} string argument was null or empty. Nothing to encrypt!");
                return string.Empty;
            }

            if (recipientPublicRsaKeyPem.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(EncryptMessageAsync)}: Message encryption failed; {nameof(recipientPublicRsaKeyPem)} string argument was null or empty. No key to encrypt with!");
                return string.Empty;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(messageJson);
                using var encryptionResult = await aes.EncryptAsync(data).ConfigureAwait(false);
                
                await using var output = new StringWriter();
                
                await output.WriteAsync(Convert.ToBase64String(rsa.Encrypt(encryptionResult.Key, recipientPublicRsaKeyPem))).ConfigureAwait(false);
                output.Write('|');
                await output.WriteAsync(Convert.ToBase64String(encryptionResult.IV)).ConfigureAwait(false);
                output.Write('|');
                await output.WriteAsync(Convert.ToBase64String(encryptionResult.EncryptedData)).ConfigureAwait(false);
                
                await output.FlushAsync().ConfigureAwait(false);
                return await compressionUtilityAsync.Compress(output.ToString()).ConfigureAwait(false);
            }
            catch
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(EncryptMessageAsync)}: Message encryption failed. Eventually an encryption key problem? Passed recipient public RSA key:\r\n{recipientPublicRsaKeyPem}");
                return null;
            }
        }
        
        /// <summary>
        /// Decrypts a message that's been encrypted using the <see cref="IMessageCryptography.EncryptMessage"/> method.
        /// </summary>
        /// <param name="encryptedMessageJson">The encrypted message <c>string</c> obtained via <see cref="IMessageCryptography.EncryptMessage"/>.</param>
        /// <param name="privateDecryptionRsaKeyPem">Your private message decryption RSA key (PEM-formatted <c>string</c>).</param>
        /// <returns>The decrypted message json; <c>null</c> if decryption failed in some way; an empty <c>string</c> if the passed arguments were <c>null</c> or empty.</returns>
        public string DecryptMessage(string encryptedMessageJson, string privateDecryptionRsaKeyPem)
        {
            if (encryptedMessageJson.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(encryptedMessageJson)} string is null or empty. Nothing to decrypt!");
                return string.Empty;
            }

            if (privateDecryptionRsaKeyPem.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(privateDecryptionRsaKeyPem)} string is null or empty. No key to decrypt with!");
                return string.Empty;
            }
            
            string[] split = compressionUtility.Decompress(encryptedMessageJson)?.Split('|');
            if (split is null ||  split.Length != 3)
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(encryptedMessageJson)} string is not in the right format! Please make sure that you do not modify the string that you obtain via the {nameof(MessageCryptography)}::{nameof(EncryptMessage)} method before passing it into this decryption method...");
                return null;
            }

            EncryptionResult encryptionResult = null;
            try
            {
                encryptionResult = new EncryptionResult
                {
                    Key = rsa.Decrypt(Convert.FromBase64String(split[0]), privateDecryptionRsaKeyPem),
                    IV = Convert.FromBase64String(split[1]),
                    EncryptedData = Convert.FromBase64String(split[2])
                };

                return Encoding.UTF8.GetString(aes.Decrypt(encryptionResult));
            }
            catch
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: Message decryption failed. Perhaps wrong or missing keys? Or invalid message format?");
                return null;
            }
            finally
            {
                encryptionResult?.Dispose();
            }
        }

        /// <summary>
        /// Asynchronously decrypts a message that's been encrypted using the <see cref="IMessageCryptography.EncryptMessageAsync"/> method.
        /// </summary>
        /// <param name="encryptedMessageJson">The encrypted message <c>string</c> obtained via <see cref="IMessageCryptography.EncryptMessageAsync"/>.</param>
        /// <param name="privateDecryptionRsaKeyPem">Your private message decryption RSA key (PEM-formatted <c>string</c>).</param>
        /// <returns>The decrypted message json; <c>null</c> if decryption failed in some way; an empty <c>string</c> if the passed arguments were <c>null</c> or empty.</returns>
        public async Task<string> DecryptMessageAsync(string encryptedMessageJson, string privateDecryptionRsaKeyPem)
        {
            if (encryptedMessageJson.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(encryptedMessageJson)} string is null or empty. Nothing to decrypt!");
                return string.Empty;
            }

            if (privateDecryptionRsaKeyPem.NullOrEmpty())
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(privateDecryptionRsaKeyPem)} string is null or empty. No key to decrypt with!");
                return string.Empty;
            }
            
            string[] split = (await compressionUtilityAsync.Decompress(encryptedMessageJson).ConfigureAwait(false))?.Split('|');
            if (split is null ||  split.Length != 3)
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(encryptedMessageJson)} string is not in the right format! Please make sure that you do not modify the string that you obtain via the {nameof(MessageCryptography)}::{nameof(EncryptMessage)} method before passing it into this decryption method...");
                return null;
            }

            EncryptionResult encryptionResult = null;
            try
            {
                encryptionResult = new EncryptionResult
                {
                    Key = rsa.Decrypt(Convert.FromBase64String(split[0]), privateDecryptionRsaKeyPem),
                    IV = Convert.FromBase64String(split[1]),
                    EncryptedData = Convert.FromBase64String(split[2])
                };

                byte[] decryptedBytes = await aes.DecryptAsync(encryptionResult).ConfigureAwait(false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch
            {
                logger?.LogError($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: Message decryption failed. Perhaps wrong or missing keys? Or invalid message format?");
                return null;
            }
            finally
            {
                encryptionResult?.Dispose();
            }
        }
    }
}
