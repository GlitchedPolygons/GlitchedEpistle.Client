using System;
using RestSharp;
using Newtonsoft.Json;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web
{
    /// <summary>
    /// Base class for all service classes that communicate
    /// with the Epistle backend Web API over HTTP requests with a request body.
    /// </summary>
    public class EpistleWebApiService
    {
        /// <summary>
        /// Creates a <see cref="RestRequest"/> using the provided <paramref name="requestBody"/>, <paramref name="endpoint"/> and <paramref name="method"/>.<para> </para>
        /// The <paramref name="requestBody"/> is serialized to JSON and added to the HTTP request body.<para> </para>
        /// Please only add bodies to POST, and maybe PUT requests...
        /// </summary>
        /// <param name="requestBody">The <see cref="EpistleRequestBody"/> to serialize into JSON and add to the HTTP POST (or PUT) request's body.</param>
        /// <param name="endpoint">The API endpoint (relative path).</param>
        /// <param name="method">The HTTP method to use. Should be either POST or PUT if you use the request body.</param>
        /// <returns>The <see cref="RestRequest"/>, ready to be submitted.</returns>
        protected RestRequest EpistleRequest(EpistleRequestBody requestBody, string endpoint, Method method = Method.POST)
        {
            var request = new RestRequest(
                method: method,
                resource: new Uri(endpoint, UriKind.Relative)
            );

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);

            return request;
        }
    }
}
