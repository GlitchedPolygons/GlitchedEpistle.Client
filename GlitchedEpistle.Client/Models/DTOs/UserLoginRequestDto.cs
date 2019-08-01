using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// HTTP request parameter DTO for user login.
    /// </summary>
    public class UserLoginRequestDto
    {
        /// <summary>
        /// The id of the <see cref="User"/> who wants to log in.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The user's password SHA512.
        /// </summary>
        [JsonProperty("pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// 2FA token.
        /// </summary>
        [JsonProperty("totp")]
        public string Totp { get; set; }
    }
}
