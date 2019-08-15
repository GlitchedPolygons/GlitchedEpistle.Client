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

using System;
using System.Collections.Generic;
using System.Text;

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ICollection{T}"/> implementations.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks whether the two <see cref="ICollection{T}"/> are equal (have the same elements).<para> </para>
        /// The order of the elements is not important; e.g. {1,2,3} and {2,3,1} would return <c>true</c>.
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> type parameter.</typeparam>
        /// <param name="a">Collection to compare.</param>
        /// <param name="b">Collection to compare.</param>
        /// <returns>Whether the two collections have the same elements.</returns>
        public static bool UnorderedEqual<T>(this ICollection<T> a, ICollection<T> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            var dictionary = new Dictionary<T, int>(a.Count);

            // Add each key's frequency from collection A to the Dictionary
            foreach (T item in a)
            {
                if (dictionary.TryGetValue(item, out int i))
                {
                    dictionary[item] = i + 1;
                }
                else
                {
                    dictionary.Add(item, 1);
                }
            }

            // Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch.
            foreach (T item in b)
            {
                if (dictionary.TryGetValue(item, out int i))
                {
                    if (i == 0)
                    {
                        return false;
                    }
                    dictionary[item] = i - 1;
                }
                else
                {
                    // Not in dictionary.
                    return false;
                }
            }

            // Verify that all frequencies are zero
            foreach (int v in dictionary.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }

            // At this point, we know that the collections are equal.
            return true;
        }
    }
}
