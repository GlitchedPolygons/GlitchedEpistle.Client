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

#region
using System;
using System.Threading.Tasks;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.ServerHealth
{
    /// <summary>
    /// Service interface for testing the connection to the epistle server.
    /// </summary>
    public interface IServerConnectionTest : IDisposable
    {
        /// <summary>
        /// Tests the connection to the epistle server.<para> </para>
        /// Returns <c>true</c> if the connection could be established or <c>false</c> if the server did not respond.
        /// </summary>
        /// <param name="serverUrl">The URL to test.</param>
        /// <returns>Whether the connection to the epistle server could be established successfully or not.</returns>
        Task<bool> TestConnection(string serverUrl = null);
    }
}
