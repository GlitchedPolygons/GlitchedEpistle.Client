/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Threading.Tasks;
using System.Collections.Generic;

using GlitchedPolygons.RepositoryPattern;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Message repository to keep on the client machine.
    /// </summary>
    public interface IMessageRepository : IRepository<Message, long>
    {
        /// <summary>
        /// Gets the <see cref="Message.Id"/> from the most recent <see cref="Message"/> in the repository.
        /// </summary>
        Task<long> GetLastMessageId();

        /// <summary>
        /// Gets the n latest <see cref="Message"/>s from the repo, optionally starting from an offset index.
        /// </summary>
        /// <param name="n">The amount of messages to retrieve.</param>
        /// <param name="offset">How many entries to skip before starting to gather messages.</param>
        Task<IEnumerable<Message>> GetLastMessages(int n, int offset = 0);
    }
}
