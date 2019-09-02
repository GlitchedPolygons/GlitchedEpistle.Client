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

namespace GlitchedPolygons.GlitchedEpistle.Client
{
    /// <summary>
    /// Class containing important <c>const</c> URLs.
    /// </summary>
    public static class URLs
    {
        /// <summary>
        /// The Glitched Epistle base URL.
        /// </summary>
        private static string epistleUrl = "https://epistle.glitchedpolygons.com/";

        /// <summary>
        /// Sets the Epistle server base url that this client connects to.
        /// </summary>
        /// <param name="url"></param>
        public static void SetEpistleServerUrl(string url)
        {
            epistleUrl = url;
        }

        /// <summary>
        /// The Glitched Epistle Web API base URL.
        /// </summary>
        public static string EpistleAPI_v1 => epistleUrl + "api/v1/";
    }
}
