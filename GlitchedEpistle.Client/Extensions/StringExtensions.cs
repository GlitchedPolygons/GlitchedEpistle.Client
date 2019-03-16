using System.Text;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// <see cref="System.String"/> extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Computes the MD5 hash of a <c>string</c>.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <param name="toLowercase">Should the output hash be lowercased?</param>
        /// <returns>MD5 hash of the input string.</returns>
        public static string MD5(this string text, bool toLowercase = false)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var stringBuilder = new StringBuilder(32);
                byte[] hash = md5.ComputeHash(text.EncodeToBytes());

                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString(toLowercase ? "x2" : "X2"));
                }

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Computes the SHA512 hash of a <c>string</c>.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <param name="toLowercase">Should the output hash <c>string</c> be lowercased?.</param>
        /// <returns>SHA512 of the input string.</returns>
        public static string SHA512(this string text, bool toLowercase = false)
        {
            using (var sha512 = System.Security.Cryptography.SHA512.Create())
            {
                var stringBuilder = new StringBuilder(128);
                byte[] hash = sha512.ComputeHash(text.EncodeToBytes());

                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString(toLowercase ? "x2" : "X2"));
                }

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Encodes a <c>string</c> to bytes (a <c>byte[]</c> array).
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to use for the conversion (default is UTF8).</param>
        /// <returns>System.Byte[].</returns>
        private static byte[] EncodeToBytes(this string text, Encoding encoding = null) => (encoding ?? Encoding.UTF8).GetBytes(text);
    }
}
