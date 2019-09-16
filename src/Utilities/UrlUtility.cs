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
using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.ServerHealth;

namespace GlitchedPolygons.GlitchedEpistle.Client.Utilities
{
    /// <summary>
    /// Class containing important <c>const</c> URLs.
    /// </summary>
    public static class UrlUtility
    {
        /// <summary>
        /// If the user does not specify http or https explicitly, should https:// prepended to the url by default or plain http?
        /// </summary>
        private const bool DEFAULT_PREPEND_SCHEME_HTTPS = true;

        private static string epistleUrl = "https://epistle.glitchedpolygons.com/";
        private static string epistleApiUrlV1 = "https://epistle.glitchedpolygons.com/api/v1/";
        private static readonly IServerConnectionTest CONNECTION_TEST = new ServerConnectionTest();

        /// <summary>
        /// This event is raised whenever the Epistle server
        /// URL was changed via the <see cref="SetEpistleServerUrl"/> method.
        /// </summary>
        public static event Action ChangedEpistleServerUrl;

        /// <summary>
        /// Sets the Epistle server base url that this client connects to.
        /// </summary>
        /// <param name="url">The new Epistle Server URL.</param>
        public static void SetEpistleServerUrl(string url)
        {
            url = FixUrl(url);

            // Only update the URL and raise the changed URL event if the connection can be established safely!
            if (CONNECTION_TEST.TestConnection(url).GetAwaiter().GetResult())
            {
                epistleUrl = url.TrimEnd('/');
                epistleApiUrlV1 = epistleUrl + "/api/v1/";
                ChangedEpistleServerUrl?.Invoke();
            }
        }

        /// <summary>
        /// Removes trailing slashes from a URL and prepends http(s):// if that's missing.<para> </para>
        /// This is NO guarantee that the connection to the url can be established!
        /// </summary>
        /// <param name="url">The url <c>string</c> to fix.</param>
        /// <returns>The fixed url <c>string</c></returns>
        public static string FixUrl(string url)
        {
            if (url is null)
            {
                return null;
            }

            url = url.TrimEnd('/');

            if (!url.Contains("http://") && !url.Contains("https://"))
            {
                url = (DEFAULT_PREPEND_SCHEME_HTTPS && !url.Contains("localhost") && !url.Contains("127.0.0.1") ? "https://" : "http://") + url;
            }

            return url;
        }

        /// <summary>
        /// The Glitched Epistle Web API URL.
        /// </summary>
        public static string EpistleAPI_v1 => epistleApiUrlV1;

        /// <summary>
        /// The Glitched Epistle server base URL.
        /// </summary>
        public static string EpistleBaseUrl => epistleUrl;
    }
}
