#region
using System;
using System.IO;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
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
        /// Generates a 4096-bit RSA key pair.<para> </para>
        /// The keys are exported into two files:
        /// Private.rsa.pem and Public.rsa.pem (inside the specified output directory).
        /// </summary>
        /// <param name="outputDirectory">The output directory path (where the keys will be exported).</param>
        /// <returns>Whether the key generation was successful or not.</returns>
        public async Task<bool> GenerateKeyPair(string outputDirectory)
        {
            if (string.IsNullOrEmpty(outputDirectory))
            {
                logger.LogError($"{nameof(AsymmetricKeygenRSA4096)}::{nameof(GenerateKeyPair)}: RSA key pair generation failed because the {nameof(outputDirectory)} string parameter was null or empty. Please give this method a valid output folder path for the keys!");
                return false;
            }

            Directory.CreateDirectory(outputDirectory);

            try
            {
                AsymmetricCipherKeyPair keyPair = await Task.Run(() =>
                {
                    var keygen = new RsaKeyPairGenerator();
                    keygen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
                    return keygen.GenerateKeyPair();
                });

                using (var sw = new StringWriter())
                {
                    var pem = new PemWriter(sw);
                    pem.WriteObject(keyPair.Private);
                    pem.Writer.Flush();

                    File.WriteAllText(Path.Combine(outputDirectory, "Private.rsa.pem"), sw.ToString());
                }

                using (var sw = new StringWriter())
                {
                    var pem = new PemWriter(sw);
                    pem.WriteObject(keyPair.Public);
                    pem.Writer.Flush();

                    File.WriteAllText(Path.Combine(outputDirectory, "Public.rsa.pem"), sw.ToString());
                }

                return true;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(AsymmetricKeygenRSA4096)}::{nameof(GenerateKeyPair)}: RSA key pair generation failed. Thrown exception: {e}");
                return false;
            }
        }
    }
}
