using System;
using System.Threading.Tasks;

using RestSharp;
using GlitchedPolygons.GlitchedEpistle.Client.Constants;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.ServerHealth
{
    /// <summary>
    /// Class for testing the connection to the epistle server.
    /// Implements the <see cref="GlitchedPolygons.GlitchedEpistle.Client.Services.ServerHealth.IServerConnectionTest" /> interface.
    /// </summary>
    /// <seealso cref="GlitchedPolygons.GlitchedEpistle.Client.Services.ServerHealth.IServerConnectionTest" />
    public class ServerConnectionTest : IServerConnectionTest
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE);

        /// <summary>
        /// Tests the connection to the epistle server.<para> </para>
        /// Returns <c>true</c> if the connection could be established or <c>false</c> if the server did not respond.
        /// </summary>
        /// <returns>Whether the connection to the epistle server could be established successfully or not.</returns>
        public async Task<bool> TestConnection()
        {
            var request = new RestRequest(
                method: Method.GET,
                resource: new Uri("marco", UriKind.Relative)
            );
            return (await restClient.ExecuteTaskAsync(request))?.Content == "polo";
        }
    }
}
