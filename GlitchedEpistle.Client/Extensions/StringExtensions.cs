using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// <c>string</c> extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Opens the <c>string</c> URL in the browser.
        /// </summary>
        /// <param name="url">The URL <see cref="string"/> to open.</param>
        public static void OpenUrlInBrowser(this string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }

        /// <summary>
        /// Returns <c>true</c> when the passed <c>string</c> is not <c>null</c> or empty; <c>false</c> otherwise.
        /// </summary>
        /// <param name="str">The <c>string</c> to check.</param>
        /// <returns><c>!string.IsNullOrEmpty(str)</c></returns>
        public static bool NotNullNotEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Returns whether the passed string is <c>null</c> or empty.
        /// </summary>
        /// <param name="str">The <c>string</c> to check.</param>
        /// <returns><c>string.IsNullOrEmpty(str)</c></returns>
        public static bool NullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

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
        /// Computes the SHA512 of a <c>string</c>.
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
        /// <returns>The encoded <c>byte[]</c> array.</returns>
        private static byte[] EncodeToBytes(this string text, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(text);
        }
    }
}
