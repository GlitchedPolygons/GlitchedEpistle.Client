namespace GlitchedPolygons.GlitchedEpistle.Client.Utilities
{
    /// <summary>
    /// Class containing important <c>const</c> URLs.
    /// </summary>
    public static class UrlUtility
    {
        private static string epistleUrl = "https://epistle.glitchedpolygons.com/";
        private static string epistleApiUrl = "https://epistle.glitchedpolygons.com/api/v1/";

        /// <summary>
        /// Sets the Epistle server base url that this client connects to.
        /// </summary>
        /// <param name="url"></param>
        public static void SetEpistleServerUrl(string url)
        {
            epistleUrl = url;
            epistleApiUrl = url + "api/v1/";
        }

        /// <summary>
        /// The Glitched Epistle Web API URL.
        /// </summary>
        public static string EpistleAPI_v1 => epistleApiUrl;

        /// <summary>
        /// The Glitched Epistle server base URL.
        /// </summary>
        public static string EpistleBaseUrl => epistleUrl;
    }
}
