using System;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// DTO for password change requests to the Epistle Web API.
    /// </summary>
    public class UserChangePasswordRequestDto
    {
        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }

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
        /// New (encrypted) private key.<para> </para>
        /// Needs to be PEM-formatted and encrypted into <c>byte[]</c> and then gzipped and base-64 encoded.
        /// </summary>
        [JsonProperty("npkey")]
        public string NewPrivateKey { get; set; }
    }
}
