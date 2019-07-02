using System.Collections.Generic;
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

        /// <summary>
        /// Gets the n latest <see cref="Message"/>s from the repo, optionally starting from an offset index.
        /// </summary>
        /// <param name="n">The amount of messages to retrieve.</param>
        /// <param name="offset">How many entries to skip before starting to gather messages.</param>
        Task<IEnumerable<Message>> GetLastMessages(int n, int offset = 0);
    }
}
