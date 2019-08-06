using Newtonsoft.Json;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// Request body for an Epistle Web API endpoint that requires authentication.
    /// </summary>
    public class EpistleRequest
    {
        /// <summary>
        /// The requesting user's ID.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The requesting user's authentication token.
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The request body. These are the endpoint parameters.<para> </para>
        /// Typically, this is some request DTO (like for example <see cref="ConvoCreationRequestDto"/>)
        /// that was serialized into JSON and gzipped.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// The request's signature.<para> </para>
        /// The signed data should be the sum of the three fields
        /// (<see cref="UserId"/> + <see cref="Auth"/> + <see cref="Body"/>).<para> </para>
        /// Only ever sign using the user's private message RSA key that the server does not have!<para> </para>
        /// <see cref="User.PrivateKeyPem"/>.
        /// <seealso cref="User"/>
        /// </summary>
        [JsonProperty("sig")]
        public string Signature { get; set; }
    }
}
