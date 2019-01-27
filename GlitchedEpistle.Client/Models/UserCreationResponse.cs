using System;
using System.Collections.Generic;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// A <see langword="class"/> containing the HTTP response data for <see cref="User"/> registration.
    /// </summary>
    public class UserCreationResponse
    {
        public string Id { get; set; }
        public string PublicKeyXml { get; set; }
        public string PasswordSHA512 { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public string Role { get; set; }
        public string TotpSecret { get; set; }
        public List<string> TotpEmergencyBackupCodes { get; set; }
        public DateTime ExpirationUTC { get; set; }
    }
}
