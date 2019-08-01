using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// Response body for successful login requests.
    /// </summary>
    public class UserLoginSuccessResponseDto
    {
        /// <summary>
        /// Valid request authentication token (jwt <see cref="System.String"/>).
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The user's public key (xml-formatted RSA key).
        /// Others need this in order to send the <see cref="User"/> messages.
        /// </summary>
        [JsonProperty("key")]
        public string PublicKeyXml { get; set; }

        /// <summary>
        /// The user's private key (xml-formatted, encrypted and base-64 encoded <see cref="System.String"/>).
        /// </summary>
        [JsonProperty("pkey")]
        public string PrivateKeyXmlEncryptedBytesBase64 { get; set; }
    }
}
