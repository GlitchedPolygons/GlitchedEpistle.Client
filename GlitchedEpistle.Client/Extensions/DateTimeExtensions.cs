using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DateTime"/> objects.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Checks whether the given <see cref="DateTime"/> is almost equal to another <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dt1">The <see cref="DateTime"/> to compare.</param>
        /// <param name="dt2">The <see cref="DateTime"/> to compare.</param>
        /// <param name="threshold">The equality-defining threshold in seconds between the two <see cref="DateTime"/>s. If the two <see cref="DateTime"/>s are further apart from each other than this amount of seconds, they're not equal.</param>
        /// <returns>Whether the two <see cref="DateTime"/> objects are almost equal.</returns>
        public static bool AlmostEquals(this DateTime dt1, DateTime dt2, double threshold = 1.0d)
        {
            TimeSpan delta = dt1 - dt2;
            return Math.Abs(delta.TotalSeconds) < threshold;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to Unix time (seconds since 1970-01-01T00:00:00Z).<para> </para>
        /// Make sure that the <see cref="DateTime"/> you're converting is UTC!
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to convert.</param>
        /// <returns>Unix time (seconds since 1970-01-01T00:00:00Z)</returns>
        public static long ToUnixTimeSeconds(this DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to Unix time (milliseconds since 1970-01-01T00:00:00Z).<para> </para>
        /// Make sure that the <see cref="DateTime"/> you're converting is UTC!
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to convert.</param>
        /// <returns>Unix time (milliseconds since 1970-01-01T00:00:00Z)</returns>
        public static long ToUnixTimeMilliseconds(this DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Converts a unix timestamp (seconds since 1970-01-01 00:00:00.000 UTC) to a UTC <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timestamp">The unix timestamp to convert.</param>
        /// <returns>The converted <see cref="DateTime"/> in UTC.</returns>
        public static DateTime FromUnixTimeSeconds(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }

        /// <summary>
        /// Converts a unix timestamp (milliseconds since 1970-01-01 00:00:00.000 UTC) to a UTC <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timestamp">The unix timestamp to convert.</param>
        /// <returns>The converted <see cref="DateTime"/> in UTC.</returns>
        public static DateTime FromUnixTimeMilliseconds(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
        }
    }
}
