#region
using System;
using System.Security.Cryptography;

using Newtonsoft.Json;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// The class that represents the epistle user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user's unique identifier (the primary key for the epistle db).
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The user's role. 
        /// </summary>
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; } = "User";

        /// <summary>
        /// The user's password hashed with SHA512.
        /// </summary>
        [JsonIgnore]
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The token needed to authenticate Web API requests.<para> </para>
        /// <see cref="Tuple{T1,T2}.Item1"/> is the token's UTC timestamp (when it was emitted).<para> </para>
        /// <see cref="Tuple{T1,T2}.Item2"/> is the token <see cref="System.String"/>.
        /// </summary>
        [JsonIgnore]
        public Tuple<DateTime, string> Token { get; set; } = null;

        /// <summary>
        /// The <see cref="DateTime"/> when this <see cref="User"/> was first created.
        /// </summary>
        [JsonProperty(PropertyName = "iat")]
        public DateTime CreationTimestampUTC { get; set; }

        /// <summary>
        /// The exact <see cref="DateTime"/> (UTC) this user's access to Epistle expires.
        /// </summary>
        [JsonProperty(PropertyName = "exp")]
        public DateTime ExpirationUTC { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The user's private message encryption RSA key.
        /// </summary>
        [JsonIgnore]
        public RSAParameters PrivateKey { get; set; }

        /// <summary>
        /// The user's public message encryption RSA key.
        /// </summary>
        [JsonIgnore]
        public RSAParameters PublicKey { get; set; }

        /// <summary>
        /// The user's public message encryption RSA key (XML-formatted, using preferably <see cref="RSA.ExportParameters"/>).
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string PublicKeyXml { get; set; }

        /// <summary>
        /// How many failed login attempts this <see cref="User"/> has on his record.
        /// After too many, he is locked out for a while.
        /// </summary>
        [JsonProperty(PropertyName = "fail")]
        public int LoginFailures { get; set; }

        /// <summary>
        /// Checks whether the <see cref="User"/>'s epistle membership is expired.
        /// </summary>
        /// <returns>Whether the <see cref="User"/>'s epistle membership is expired or not.</returns>
        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationUTC;
        }
    }
}
