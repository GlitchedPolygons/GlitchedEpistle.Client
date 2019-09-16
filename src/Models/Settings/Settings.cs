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
    /// The base class for all settings to be provided to Glitched Epistle client implementations.
    /// </summary>
    public abstract class Settings
    {
        /// <summary>
        /// Saves the current user settings out to disk.
        /// </summary>
        /// <returns>Whether the settings were saved out to disk successfully or not.</returns>
        public abstract bool Save();

        /// <summary>
        /// Loads user settings from disk into the <see cref="Settings"/> instance.
        /// </summary>
        /// <returns>Whether the loading procedure was successful or not.</returns>
        public abstract bool Load();
    }
}