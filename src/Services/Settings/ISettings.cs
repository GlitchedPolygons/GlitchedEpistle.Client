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
    /// Service interface for accessing, saving and loading user settings.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets or sets a user setting with its specified key <c>string</c>.<para> </para>
        /// Setting should also auto-save the config.<para> </para>
        /// If you are trying to get an inexistent setting, <c>null</c> (or <c>string.Empty</c>) should be returned.<para> </para>
        /// If you are trying to set an inexistent setting, the setting shall be created.
        /// </summary>
        /// <param name="key">The setting's name/key.</param>
        /// <returns>The setting's <c>string</c> value; <c>null</c> (or <c>string.Empty</c>) if the setting doesn't exist.</returns>
        string this[string key] { get; set; }

        /// <summary>
        /// Gets a user setting by its key <c>string</c>.
        /// </summary>
        /// <param name="key">The setting's name/key.</param>
        /// <param name="defaultValue">The setting's default <c>string</c> value (in case the setting doesn't exist).</param>
        /// <returns>The setting's <c>string</c> value; the specified default value if the setting wasn't found.</returns>
        string this[string key, string defaultValue] { get; }

        /// <summary>
        /// Gets a user setting parsed as an <c>int</c>.
        /// </summary>
        /// <param name="key">The setting's key.</param>
        /// <param name="defaultValue">The setting's default <c>int</c> value to return in case the setting doesn't exist or couldn't be parsed.</param>
        /// <returns>The setting's <c>int</c> value; or the specified default value if the setting wasn't found or couldn't be parsed.</returns>
        int this[string key, int defaultValue] { get; }

        /// <summary>
        /// Gets a user setting parsed as a <c>bool</c>.
        /// </summary>
        /// <param name="key">The setting's key.</param>
        /// <param name="defaultValue">The setting's default <c>bool</c> value to return in case the setting doesn't exist or couldn't be parsed.</param>
        /// <returns>The setting's <c>bool</c> value; or the specified default value if the setting wasn't found or couldn't be parsed.</returns>
        bool this[string key, bool defaultValue] { get; }

        /// <summary>
        /// Gets a user setting parsed as a <c>float</c>.
        /// </summary>
        /// <param name="key">The setting's key.</param>
        /// <param name="defaultValue">The setting's default <c>float</c> value to return in case the setting doesn't exist or couldn't be parsed.</param>
        /// <returns>The setting's <c>float</c> value; or the specified default value if the setting wasn't found or couldn't be parsed.</returns>
        float this[string key, float defaultValue] { get; }

        /// <summary>
        /// Gets a user setting parsed as a <c>double</c>.
        /// </summary>
        /// <param name="key">The setting's key.</param>
        /// <param name="defaultValue">The setting's default <c>double</c> value to return in case the setting doesn't exist or couldn't be parsed.</param>
        /// <returns>The setting's <c>double</c> value; or the specified default value if the setting wasn't found or couldn't be parsed.</returns>
        double this[string key, double defaultValue] { get; }
    }
}
