using System;
using System.IO;
using System.Xml;
using System.Security.Cryptography;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Class holding <see cref="RSAParameters"/> extension methods.
    /// </summary>
    public static class RSAParametersExtensions
    {
        /// <summary>
        /// Deserializes an <see cref="RSAParameters"/> instance from an xml <c>string</c>.
        /// </summary>
        /// <param name="xml">The XML containing the <see cref="RSAParameters"/>.</param>
        /// <returns><see cref="RSAParameters"/>.</returns>
        /// <exception cref="InvalidDataException">Invalid XML RSA key.</exception>
        public static RSAParameters FromXml(string xml)
        {
            var rsaParameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            if (xmlDoc.DocumentElement != null && xmlDoc.DocumentElement.Name == "RSAKeyValue")
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": rsaParameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": rsaParameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": rsaParameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": rsaParameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": rsaParameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": rsaParameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": rsaParameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": rsaParameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new InvalidDataException("Invalid XML RSA key.");
            }

            return rsaParameters;
        }

        /// <summary>
        /// Converts <see cref="RSAParameters"/> to xml.
        /// </summary>
        /// <param name="parameters">The key to convert to xml.</param>
        /// <returns>System.String.</returns>
        public static string ToXmlString(this RSAParameters parameters)
        {
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }
    }
}
