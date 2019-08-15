/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

#region
using System;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Extension methods for <c>byte[]</c> arrays.
    /// </summary>
    public static class ByteArrayExtensions
    {
        private static readonly string[] SIZE_SUFFIX_STRINGS = { " B", " KB", " MB", " GB", " TB", " PB", " EB" }; // Longs run out around EB

        /// <summary>
        /// Gets the <c>byte[]</c> array's human readable file size string (e.g. 5KB or 20MB).
        /// </summary>
        /// <param name="bytes">The bytes (for example a file whose size you want to retrieve in a human readable way).</param>
        /// <returns>A human readable file size <c>string</c> that represents the passed <c>byte[]</c> array's length (<c>string.Empty</c> if the array was <c>null</c>).</returns>
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
            return (Math.Sign(byteCount) * n).ToString() + SIZE_SUFFIX_STRINGS[i];
        }
    }
}
