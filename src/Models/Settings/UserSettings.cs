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

namespace GlitchedPolygons.GlitchedEpistle.Client.Models.Settings
{
    /// <summary>
    /// Per-user, account-level settings that change for every user account
    /// (when you log out and then in again with another account, these should be reloaded!).
    /// </summary>
    public abstract class UserSettings : Settings
    {
        /// <summary>
        /// The username to use for sending messages. 
        /// </summary>
        public abstract string Username { get; set; }
    }
}