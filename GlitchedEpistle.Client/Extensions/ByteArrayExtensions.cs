using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Extension methods for <c>byte[]</c> arrays.
    /// </summary>
    public static class ByteArrayExtensions
    {
        private static readonly string[] SIZE_SUFFIX_STRINGS = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; // Longs run out around EB

        /// <summary>
        /// Gets the <c>byte[]</c> array's human readable file size string (e.g. 5KB or 20MB).
        /// </summary>
        /// <param name="bytes">The bytes (for example a file whose size you want to retrieve in a human readable way).</param>
        /// <returns>A human readable file size <c>string</c> that represents the passed <c>byte[]</c> array's length (<c>string.Empty</c> if the array was <see langword="null"/>).</returns>
        public static string GetFileSizeString(this byte[] bytes)
        {
            if (bytes is null)
            {
                return string.Empty;
            }
            long byteCount = Math.Abs(bytes.LongLength);
            if (byteCount == 0)
            {
                return "0" + SIZE_SUFFIX_STRINGS[0];
            }
            int i = Convert.ToInt32(Math.Floor(Math.Log(byteCount, 1024)));
            double n = Math.Round(byteCount / Math.Pow(1024, i), 1);
            return (Math.Sign(byteCount) * n) + SIZE_SUFFIX_STRINGS[i];
        }
    }
}
