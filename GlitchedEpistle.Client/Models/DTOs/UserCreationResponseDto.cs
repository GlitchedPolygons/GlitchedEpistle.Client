using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// A <see langword="class"/> containing the HTTP response data for <see cref="User"/> registration.
    /// </summary>
    public class UserCreationResponseDto
    {
        /// <summary>
        /// The user's unique identifier (the primary key for the epistle db).
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The user's password hashed with SHA512.
        /// </summary>
        [JsonProperty(PropertyName = "pw")]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The <see cref="DateTime"/> when this <see cref="User"/> was first created.
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// The user's role. 
        /// </summary>
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        /// <summary>
        /// The user's 2FA TOTP secret.
        /// </summary>
        /// <value>The totp secret.</value>
        [JsonProperty(PropertyName = "totps")]
        public string TotpSecret { get; set; }

        /// <summary>
        /// The user's 2FA TOTP emergency backup codes (can only be used once).
        /// </summary>
        /// <value>The 2FA TOTP emergency backup codes.</value>
        [JsonProperty(PropertyName = "sos")]
        public List<string> TotpEmergencyBackupCodes { get; set; }

        /// <summary>
        /// The exact <see cref="DateTime"/> (UTC) this user's access to Epistle expires.
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; }
    }
}
