#region
using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

using GlitchedPolygons.GlitchedEpistle.Client.Extensions;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.ExtensionMethods.RSAXmlPemStringConverter;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// 4096-bit RSA key generator.
    /// Implements the <see cref="IAsymmetricKeygen" />
    /// </summary>
    /// <seealso cref="IAsymmetricKeygen" />
    public class AsymmetricKeygenRSA4096 : IAsymmetricKeygen
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricKeygenRSA4096"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance with which any eventual problems/errors will be logged.</param>
        public AsymmetricKeygenRSA4096(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException($"{nameof(AsymmetricKeygenRSA4096)}::ctor: The passed {nameof(logger)} argument is null! Please provide a valid, reliable {nameof(ILogger)} instance to this constructor!");
        }

        /// <summary>
        /// Generates a 4096-bit RSA key pair.
        /// </summary>
        /// <returns>The RSA key pair <see cref="Tuple"/>, where the first item is the public key and the second is the private key. If generation failed for some reason, <c>null</c> is returned.</returns>
        public Task<Tuple<RSAParameters, RSAParameters>> GenerateKeyPair()
        {
            return Task.Run(() =>
            {
                try
                {
                    var keygen = new RsaKeyPairGenerator();
                    keygen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
                    AsymmetricCipherKeyPair keyPair = keygen.GenerateKeyPair();
                    return new Tuple<RSAParameters, RSAParameters>(GetRSAParameters(keyPair.Public), GetRSAParameters(keyPair.Private));
                }
                catch (Exception e)
                {
                    logger.LogError($"{nameof(AsymmetricKeygenRSA4096)}::{nameof(GenerateKeyPair)}: RSA key pair generation failed. Thrown exception: {e}");
                    return null;
                }
            });
        }

        private RSAParameters GetRSAParameters(AsymmetricKeyParameter key)
        {
            using (var sw = new StringWriter())
            {
                var pem = new PemWriter(sw);
                pem.WriteObject(key);
                pem.Writer.Flush();
                return RSAParametersExtensions.FromXmlString(sw.ToString().PemToXml());
            }
        }
    }
}
