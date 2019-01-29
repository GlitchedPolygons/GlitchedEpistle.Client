using System;
using System.IO;
using System.Threading.Tasks;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// 4096-bit RSA key generator.
    /// Implements the <see cref="IAsymmetricKeygen" />
    /// </summary>
    /// <seealso cref="IAsymmetricKeygen" />
    public class RSA4096Keygen : IAsymmetricKeygen
    {
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
                return false;
            }

            try
            {

                Directory.CreateDirectory(outputDirectory);

                await Task.Run(() =>
                {
                    var keygen = new RsaKeyPairGenerator();
                    keygen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
                    var keys = keygen.GenerateKeyPair();

                    using (var sw = new StringWriter())
                    {
                        var pem = new PemWriter(sw);
                        pem.WriteObject(keys.Private);
                        pem.Writer.Flush();

                        File.WriteAllText(Path.Combine(outputDirectory, "Private.rsa.pem"), sw.ToString());
                    }

                    using (var sw = new StringWriter())
                    {
                        var pem = new PemWriter(sw);
                        pem.WriteObject(keys.Public);
                        pem.Writer.Flush();

                        File.WriteAllText(Path.Combine(outputDirectory, "Public.rsa.pem"), sw.ToString());
                    }
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
