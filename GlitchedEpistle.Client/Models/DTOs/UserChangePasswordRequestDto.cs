using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for password change requests to the Epistle Web API.
    /// </summary>
    public class UserChangePasswordRequestDto
    {
        /// <summary>
        /// The <see cref="User"/> id of who wants to change his password.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Request authentication token.
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// Old password SHA512.
        /// </summary>
        [JsonProperty("oldPwSHA512")]
        public string OldPwSHA512 { get; set; }

        /// <summary>
        /// New password's SHA512.
        /// </summary>
        [JsonProperty("newPwSHA512")]
        public string NewPwSHA512 { get; set; }

        /// <summary>
        /// New (encrypted) private key.
        /// </summary>
        [JsonProperty("npkey")]
        public string NewPrivateKeyXmlEncryptedBytesBase64 { get; set; }
    }
}
