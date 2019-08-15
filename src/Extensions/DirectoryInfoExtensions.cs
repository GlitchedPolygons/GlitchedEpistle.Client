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
using System.IO;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Extensions
{
    /// <summary>
    /// Class that holds <see cref="DirectoryInfo"/> extension methods.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Deletes the specified directory recursively,
        /// including all of its sub-directories and files.
        /// </summary>
        /// <param name="dir">The directory to delete.</param>
        public static void DeleteRecursively(this DirectoryInfo dir)
        {
            if (dir is null || !dir.Exists)
            {
                return;
            }

            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                DeleteRecursively(subDir);
                subDir.Delete();
            }
        }
    }
}
