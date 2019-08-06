#region
using System;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Constants;

using RestSharp;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.ServerHealth
{
    /// <summary>
    /// Class for testing the connection to the epistle server.
    /// Implements the <see cref="IServerConnectionTest" /> interface.
    /// </summary>
    /// <seealso cref="IServerConnectionTest" />
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
