using System;

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
            if (!url.Contains("http://") && !url.Contains("https://"))
            {
                url = DEFAULT_PREPEND_SCHEME_HTTPS ? "https://" : "http://" + url;
            }
            epistleUrl = url.TrimEnd('/');
            epistleApiUrlV1 = url + "/api/v1/";
            ChangedEpistleServerUrl?.Invoke();
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
