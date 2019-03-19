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
