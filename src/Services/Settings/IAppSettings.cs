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

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Settings
{
    /// <summary>
    /// Application-level settings that persist between user account switches.
    /// </summary>
    public interface IAppSettings : ISettings
    {
        /// <summary>
        /// The Epistle client version.
        /// </summary>
        string ClientVersion { get; }
        
        /// <summary>
        /// The Epistle server URL to connect to.
        /// </summary>
        string ServerUrl { get; set; }
        
        /// <summary>
        /// The last used user id.
        /// </summary>
        string LastUserId { get; set; }
    }
}