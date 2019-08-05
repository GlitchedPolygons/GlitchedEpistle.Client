using System;
using System.IO;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Org.BouncyCastle.Crypto.AsymmetricKeyParameter"/>. 
    /// </summary>
    public static class AsymmetricKeyParameterExtensions
    {
        /// <summary>
        /// Converts a BouncyCastle <see cref="AsymmetricKeyParameter"/> to <c>string</c> (PEM-formatted).
        /// </summary>
        /// <param name="key">The key to stringify.</param>
        /// <returns><see cref="String"/> containing the PEM-formatted key.</returns>
        public static string ToPemString(this AsymmetricKeyParameter key)
        {
            using (var sw = new StringWriter())
            {
                var pem = new PemWriter(sw);
                pem.WriteObject(key);
                pem.Writer.Flush();
                return sw.ToString();
            }
        }
    }
}
