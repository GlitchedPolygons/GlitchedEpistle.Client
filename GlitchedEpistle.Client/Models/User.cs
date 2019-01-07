using System;

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
        public string Id { get; set; }

        /// <summary>
        /// The user's password hashed with SHA512.
        /// </summary>
        public string PasswordSHA512 { get; set; }

        /// <summary>
        /// The token needed to authenticate Web API requests.
        /// </summary>
        /// <value>Encoded JWT <see langword="string"/>.</value>
        public string Token { get; set; }

        /// <summary>
        /// The exact <see cref="DateTime"/> (UTC) this user's access to Epistle expires.
        /// </summary>
        public DateTime ExpirationUTC { get; set; } = DateTime.MinValue;
    }
}
