using GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    /// <summary>
    /// HTTP PUT request DTO for kicking a <see cref="User"/> out of a <see cref="Convo"/>.
    /// <seealso cref="IConvoService.KickUser"/>
    /// </summary>
    public class ConvoKickUserRequestDto
    {
        /// <summary>
        /// The convo's identifier.
        /// </summary>
        public string ConvoId { get; set; }

        /// <summary>
        /// The convo's password hashed using SHA512.
        /// </summary>
        public string ConvoPasswordSHA512 { get; set; }

        /// <summary>
        /// The user id of who you're kicking out.
        /// </summary>
        public string UserIdToKick { get; set; }

        /// <summary>
        /// If set to <c>true</c>, the kicked user won't be able to rejoin the convo permanently.
        /// </summary>
        public bool PermaBan { get; set; }
    }
}
