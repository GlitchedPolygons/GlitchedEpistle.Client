/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

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
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Service interface for submitting <see cref="Convo"/> messages to the Epistle server.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends a text message to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into (will use the <see cref="Convo.Id"/> and other credentials for establishing a connection to the Epistle server).</param>
        /// <param name="message"></param>
        /// <returns>Whether the message submission was successful or failed.</returns>
        Task<bool> PostText(Convo convo, string message);

        /// <summary>
        /// Submits a file to a <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to post the message into (will use the <see cref="Convo.Id"/> and other credentials for establishing a connection to the Epistle server).</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileBytes">File <c>byte[]</c> </param>
        /// <returns>Whether the message could be submitted successfully to the backend or not.</returns>
        Task<bool> PostFile(Convo convo, string fileName, byte[] fileBytes);
    }
}
