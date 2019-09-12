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
        private static IServerConnectionTest connectionTest = new ServerConnectionTest();

        /// <summary>
        /// This event is raised whenever the Epistle server
        /// URL was changed via the <see cref="SetEpistleServerUrl"/> method.
        /// </summary>
        public static event Action ChangedEpistleServerUrl;

        /// <summary>
        /// Sets the Epistle server base url that this client connects to.
        /// </summary>
        /// <param name="url">The new Epistle Server URL. </param>
        public static void SetEpistleServerUrl(string url)
        {
            url = FixUrl(url);

            // Only update the URL and raise the changed URL event if the connection can be established safely!
            if (connectionTest.TestConnection(url).GetAwaiter().GetResult())
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
            if (!url.Contains("http://") && !url.Contains("https://"))
            {
                url = (DEFAULT_PREPEND_SCHEME_HTTPS ? "https://" : "http://") + url;
            }
            return url.TrimEnd('/');
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
