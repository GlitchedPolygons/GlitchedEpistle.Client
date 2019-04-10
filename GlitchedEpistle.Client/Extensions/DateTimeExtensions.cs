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
    }
}
