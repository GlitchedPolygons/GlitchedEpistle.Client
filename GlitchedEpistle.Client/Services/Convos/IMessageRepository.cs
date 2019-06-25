using System.Threading.Tasks;

using GlitchedPolygons.RepositoryPattern;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Message repository.
    /// </summary>
    public interface IMessageRepository : IRepository<Message, string>
    {
        /// <summary>
        /// Gets the <see cref="Message.Id"/> from the most recent <see cref="Message"/> in the repository.
        /// </summary>
        Task<string> GetLastMessageId();
    }
}
