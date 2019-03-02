﻿using System;
using System.Text;
using System.IO.Compression;
using System.Security.Cryptography;

using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages
{
    /// <summary>
    /// Service interface implementation for comfortably encrypting/decrypting epistle messages (usually json strings).
    /// </summary>
    /// <seealso cref="GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages.IMessageCryptography" />
    public class MessageCryptography : IMessageCryptography
    {
        private static readonly CompressionSettings COMPRESSION_SETTINGS = new CompressionSettings { CompressionLevel = CompressionLevel.Optimal };

        private readonly ICompressionUtility gzip;
        private readonly ISymmetricCryptography aes;
        private readonly IAsymmetricCryptographyRSA rsa;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCryptography"/> class.
        /// </summary>
        /// <param name="aes">The <see cref="ISymmetricCryptography"/> instance (should be injected via IoC).</param>
        /// <param name="rsa">The <see cref="IAsymmetricCryptographyRSA"/> instance (should be injected via IoC).</param>
        /// <param name="gzip">The <see cref="ICompressionUtility"/> instance needed for compression (should be injected via IoC).</param>
        public MessageCryptography(ISymmetricCryptography aes, IAsymmetricCryptographyRSA rsa, ICompressionUtility gzip)
        {
            this.aes = aes;
            this.rsa = rsa;
            this.gzip = gzip;
        }

        /// <summary>
        /// Encrypts a message json <c>string</c> for a specific recipient, whose public encryption RSA key you know (xml <c>string</c>).
        /// </summary>
        /// <param name="messageJson">The message json (<c>string</c>) to encrypt.</param>
        /// <param name="recipientPublicRsaKey">The recipient's public RSA key (used for encryption).</param>
        /// <returns>The encrypted message <c>string</c>.</returns>
        public string EncryptMessage(string messageJson, RSAParameters recipientPublicRsaKey)
        {
            byte[] data = gzip.Compress(Encoding.UTF8.GetBytes(messageJson), COMPRESSION_SETTINGS);
            using (var encryptionResult = aes.Encrypt(data))
            {
                var stringBuilder = new StringBuilder(encryptionResult.encryptedData.Length);
                stringBuilder.Append(Convert.ToBase64String(rsa.Encrypt(encryptionResult.key, recipientPublicRsaKey)));
                stringBuilder.Append('|');
                stringBuilder.Append(Convert.ToBase64String(encryptionResult.iv));
                stringBuilder.Append('|');
                stringBuilder.Append(Convert.ToBase64String(encryptionResult.encryptedData));
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Decrypts a message that's been encrypted using the <see cref="EncryptMessage(string,RSAParameters)" /> method.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message <see langword="string"/> obtained via <see cref="EncryptMessage"/>.</param>
        /// <param name="privateDecryptionRsaKey">Your private message decryption RSA key.</param>
        /// <returns>The decrypted message json (or <see langword="null" /> if decryption failed in some way).</returns>
        public string DecryptMessage(string encryptedMessage, RSAParameters privateDecryptionRsaKey)
        {
            string[] split = encryptedMessage.Split('|');
            if (split.Length != 3)
            {
                throw new ArgumentException($"{nameof(MessageCryptography)}::{nameof(DecryptMessage)}: The provided {nameof(encryptedMessage)} string is not in the right format! Please make sure that you do not modify the string that you obtain via the {nameof(MessageCryptography)}::{nameof(EncryptMessage)} method before passing it into this decryption method...");
            }

            var encryptionResult = new EncryptionResult
            {
                key = rsa.Decrypt(Convert.FromBase64String(split[0]), privateDecryptionRsaKey),
                iv = Convert.FromBase64String(split[1]),
                encryptedData = Convert.FromBase64String(split[2])
            };

            byte[] decryptedDecompressed = gzip.Decompress(aes.Decrypt(encryptionResult), COMPRESSION_SETTINGS);
            encryptionResult.Dispose();
            string result = Encoding.UTF8.GetString(decryptedDecompressed);
            for (int i = 0; i < decryptedDecompressed.Length; i++) decryptedDecompressed[i] = 0;
            return result;
        }
    }
}
