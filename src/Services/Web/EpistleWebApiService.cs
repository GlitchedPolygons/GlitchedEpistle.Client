/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

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
using System.Text.Json;

using RestSharp;
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
        protected RestRequest EpistleRequest(EpistleRequestBody requestBody, string endpoint, Method method = Method.Post)
        {
            if (method != Method.Post && method != Method.Put)
            {
                throw new ArgumentException($"{nameof(EpistleWebApiService)}::{nameof(EpistleRequest)}: Non-PUT or POST HTTP method passed. Please only add request bodies to POST and PUT requests!");
            }

            var request = new RestRequest(
                method: method,
                resource: new Uri(endpoint, UriKind.Relative)
            );

            request.AddParameter("application/json", JsonSerializer.Serialize(requestBody), ParameterType.RequestBody);

            return request;
        }
    }
}
