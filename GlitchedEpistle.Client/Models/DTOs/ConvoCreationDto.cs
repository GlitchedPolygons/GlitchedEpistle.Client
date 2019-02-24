using System;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs
{
    public class ConvoCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PasswordSHA512 { get; set; }
        public DateTime Expires { get; set; } = DateTime.MaxValue;
    }
}
