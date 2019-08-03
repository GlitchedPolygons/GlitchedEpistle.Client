using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Parameters;

using GlitchedPolygons.GlitchedEpistle.Client.Extensions;

using Org.BouncyCastle.Security;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// <see cref="IAsymmetricCryptographyRSA"/> implementation for asymmetric RSA encryption/decryption.
    /// </summary>
    /// <seealso cref="IAsymmetricCryptographyRSA" />
    public class AsymmetricCryptographyRSA : IAsymmetricCryptographyRSA
    {
        /// <summary>
        /// The algorithm used for signing and verifying.
        /// <seealso cref="SignerUtilities"/>
        /// </summary>
        private const string SIGNATURE_ALGO = "SHA256withRSA";
        
        /// <summary>
        /// Tries to convert a PEM-formatted <c>string</c> => <see cref="AsymmetricCipherKeyPair"/>.<para> </para>
        /// Only possible if the provided key is the private key (public keys are typically read with the <see cref="PemReader"/> as <see cref="RsaKeyParameters"/>).
        /// </summary>
        /// <param name="rsaKeyPem">The PEM-formatted key <c>string</c> to convert.</param>
        /// <returns>The converted <see cref="AsymmetricCipherKeyPair"/>; <c>null</c> if the provided key <c>string</c> was <c>null</c>, empty or the public key.</returns>
        private static AsymmetricCipherKeyPair PemStringToKeyPair(string rsaKeyPem)
        {
            if (rsaKeyPem.NullOrEmpty())
            {
                return null;
            }
            
            var stringReader = new StringReader(rsaKeyPem);
            try
            {
                var pemReader = new PemReader(stringReader);
                return pemReader.ReadObject() as AsymmetricCipherKeyPair;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                stringReader.Dispose();
            }
        }

        /// <summary>
        /// Tries to convert a PEM-formatted <c>string</c> => <see cref="RsaKeyParameters"/>.<para> </para>
        /// </summary>
        /// <param name="rsaKeyPem">The PEM-formatted key <c>string</c> to convert.</param>
        /// <returns>The converted <see cref="RsaKeyParameters"/>; <c>null</c> if the provided key <c>string</c> was <c>null</c> or empty.</returns>
        private static RsaKeyParameters PemStringToKeyParameters(string rsaKeyPem)
        {
            if (rsaKeyPem.NullOrEmpty())
            {
                return null;
            }
            
            var stringReader = new StringReader(rsaKeyPem);
            try
            {
                var pemReader = new PemReader(stringReader);
                return pemReader.ReadObject() as RsaKeyParameters;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                stringReader.Dispose();
            }
        }
        
        /// <summary>
        /// Encrypts or decrypts the input <paramref name="data"/> parameter
        /// according to the <paramref name="encrypt"/> <c>bool</c> parameter, using the provided RSA <paramref name="key"/>.<para> </para>
        /// If <paramref name="encrypt"/> is set to <c>false</c>, the method will try to decrypt instead.<para> </para>
        /// This method can throw exceptions! E.g. don't pass any <c>null</c> or invalid arguments.
        /// Trying to decrypt with a <c>null</c> or public <paramref name="key"/> will throw exceptions! Make sure to wrap the call to this method in a try/catch block.
        /// </summary>
        /// <param name="data">The data to encrypt or decrypt</param>
        /// <param name="key">The RSA key to use for encryption/decryption.</param>
        /// <param name="encrypt">Should the method encrypt the passed input <paramref name="data"/> or attempt to decrypt it?</param>
        /// <returns>The processed data <c>byte[]</c> array; exceptions are thrown in case of a failure.</returns>
        private static byte[] ProcessData(byte[] data, ICipherParameters key, bool encrypt)
        {
            // PKCS1 OAEP paddings
            OaepEncoding eng = new OaepEncoding(new RsaEngine());
            eng.Init(encrypt, key);

            int length = data.Length;
            int blockSize = eng.GetInputBlockSize();

            List<byte> processedBytes = new List<byte>(length);

            for (int chunkPosition = 0; chunkPosition < length; chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                processedBytes.AddRange(eng.ProcessBlock(data, chunkPosition, chunkSize));
            }

            return processedBytes.ToArray();
        }
        
        #region Encrypting and decrypting
        
        /// <summary>
        /// Encrypts the specified text using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="text">The plain text to encrypt.</param>
        /// <param name="publicKeyPem">The public RSA key for encryption (PEM-formatted <c>string</c>).</param>
        /// <returns>The encrypted <c>string</c>; <c>string.Empty</c> if the passed key or plain text argument was <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        public string Encrypt(string text, string publicKeyPem)
        {
            if (text.NullOrEmpty() || publicKeyPem.NullOrEmpty())
            {
                return string.Empty;
            }
            try
            {
                return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(text), publicKeyPem));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified text using the provided RSA private key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="privateKeyPem">The private RSA key needed for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted <c>string</c>; <c>null</c> if the passed key or encrypted text argument was <c>null</c> or empty; <c>null</c> if decryption failed.</returns>
        public string Decrypt(string encryptedText, string privateKeyPem)
        {
            if (encryptedText.NullOrEmpty() || privateKeyPem.NullOrEmpty())
            {
                return string.Empty;
            }
            try
            {
                return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptedText), privateKeyPem));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Encrypts the specified bytes using the provided RSA public key, which needs to be a PEM-formatted <c>string</c>.
        /// </summary>
        /// <param name="data">The data (<c>byte[]</c> array) to encrypt.</param>
        /// <param name="publicKeyPem">The public key (PEM-formatted <c>string</c>) to use for encryption.</param>
        /// <returns>The encrypted bytes (<c>System.Byte[]</c>); <c>Array.Empty&lt;byte&gt;()</c> if the passed data or key argument was <c>null</c> or empty; <c>null</c> if encryption failed.</returns>
        public byte[] Encrypt(byte[] data, string publicKeyPem)
        {
            if (data is null || data.Length == 0 || publicKeyPem.NullOrEmpty())
            {
                return Array.Empty<byte>();
            }

            try
            {
                AsymmetricCipherKeyPair keyPair = PemStringToKeyPair(publicKeyPem);
                ICipherParameters key = keyPair?.Public ?? PemStringToKeyParameters(publicKeyPem);

                return ProcessData(data, key, true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified bytes using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).
        /// </summary>
        /// <param name="encryptedData">The encrypted data bytes (<c>byte[]</c>).</param>
        /// <param name="privateKeyPem">The private RSA key to use for decryption (PEM-formatted <c>string</c>).</param>
        /// <returns>Decrypted bytes (System.Byte[]) if successful; an empty <c>byte[]</c> array if the passed data or key argument was <c>null</c> or empty; <c>null</c> if decryption failed.</returns>
        public byte[] Decrypt(byte[] encryptedData, string privateKeyPem)
        {
            if (encryptedData is null || encryptedData.Length == 0 || privateKeyPem.NullOrEmpty())
            {
                return Array.Empty<byte>();
            }

            try
            {
                return ProcessData(encryptedData, PemStringToKeyPair(privateKeyPem).Private, false);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        #endregion
        
        #region Signing and verifying
        
        /// <summary>
        /// Signs the specified <c>string</c> using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// Signature algo is the value of <see cref="SIGNATURE_ALGO"/>; see <see cref="SignerUtilities"/> for more information about what algorithms are supported and what <c>string</c>s to use here.<para> </para>
        /// If the procedure succeeds, the calculated signature <c>string</c> is returned (which is base-64 encoded).<para> </para>
        /// Otherwise, an empty <c>string</c> is returned if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters
        /// were <c>null</c> or empty. If the procedure fails entirely, <c>null</c> is returned.
        /// </summary>
        /// <param name="data">The data to sign.</param>
        /// <param name="privateKeyPem">The private RSA key to use for generating the signature (PEM-formatted <c>string</c>)</param>
        /// <returns>The signature (base-64 encoded <c>string</c>). <c>string.Empty</c> is returned if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters were <c>null</c> or empty. Returns <c>null</c> if signing failed entirely.</returns>
        /// <seealso cref="SignerUtilities"/>
        public string Sign(string data, string privateKeyPem)
        {
            if (data.NullOrEmpty() || privateKeyPem.NullOrEmpty())
            {
                return string.Empty;
            }

            try
            {
                byte[] signature = Sign(Encoding.UTF8.GetBytes(data), privateKeyPem);
                return Convert.ToBase64String(signature);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Verifies a signature that was obtained using <see cref="Sign(string,string)"/>
        /// with a public RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// </summary>
        /// <param name="data">The data whose signature you want to verify.</param>
        /// <param name="signature">The passed <paramref name="data"/>'s signature (return value of <see cref="Sign(string,string)"/>).</param>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to use for signature verification.</param>
        /// <returns>Whether the data's signature verification succeeded or not.</returns>
        public bool Verify(string data, string signature, string publicKeyPem)
        {
            if (data.NullOrEmpty() || signature.NullOrEmpty() || publicKeyPem.NullOrEmpty())
            {
                return false;
            }
            try
            {
                byte[] signatureBytes = Convert.FromBase64String(signature);
                return Verify(Encoding.UTF8.GetBytes(data), signatureBytes, publicKeyPem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Signs the specified data <c>byte[]</c> array using the provided private RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// If the procedure succeeds, the calculated signature <c>byte[]</c> array is returned. Otherwise,
        /// an empty <c>byte[]</c> array is returned if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters
        /// were <c>null</c> or empty. If the procedure fails entirely, <c>null</c> is returned.
        /// </summary>
        /// <param name="data">The data to sign.</param>
        /// <param name="privateKeyPem">The private RSA key to use for generating the signature (PEM-formatted <c>string</c>).</param>
        /// <returns>The signature (<c>byte[]</c>), <c>string.Empty</c> if the provided <paramref name="data"/> and/or <paramref name="privateKeyPem"/> parameters were <c>null</c> or empty. Returns <c>null</c> if signing failed entirely.</returns>
        public byte[] Sign(byte[] data, string privateKeyPem)
        {
            if (data is null || data.Length == 0 || privateKeyPem.NullOrEmpty())
            {
                return Array.Empty<byte>();
            }
            try
            {
                ISigner sig = SignerUtilities.GetSigner(SIGNATURE_ALGO);
                
                sig.Init(true, PemStringToKeyPair(privateKeyPem).Private);
                sig.BlockUpdate(data, 0, data.Length);
                
                return sig.GenerateSignature();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Verifies a signature that was obtained using <see cref="Sign(byte[],string)"/> with a public RSA key (which needs to be a PEM-formatted <c>string</c>).<para> </para>
        /// </summary>
        /// <param name="data">The data whose signature you want to verify.</param>
        /// <param name="signature">The passed <paramref name="data"/>'s signature (return value of <see cref="Sign(byte[],string)"/>).</param>
        /// <param name="publicKeyPem">The public RSA key (PEM-formatted) to use for signature verification.</param>
        /// <returns>Whether the data's signature verification succeeded or not.</returns>
        public bool Verify(byte[] data, byte[] signature, string publicKeyPem)
        {
            if (data is null || data.Length == 0 || publicKeyPem.NullOrEmpty())
            {
                return false;
            }
            try
            {
                ISigner signer = SignerUtilities.GetSigner(SIGNATURE_ALGO);
                
                signer.Init(false, PemStringToKeyParameters(publicKeyPem));
                signer.BlockUpdate(data, 0, data.Length);
                
                return signer.VerifySignature(signature);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #endregion
    }
}