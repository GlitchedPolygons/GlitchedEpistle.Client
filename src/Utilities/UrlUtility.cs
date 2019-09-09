using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Utilities
{
    /// <summary>
    /// Class containing important <c>const</c> URLs.
    /// </summary>
    public static class UrlUtility
    {
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
            epistleUrl = url;
            epistleApiUrlV1 = url.TrimEnd('/') + "/api/v1/";
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
