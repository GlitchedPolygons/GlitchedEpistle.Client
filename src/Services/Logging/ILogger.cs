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

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Logging
{
    /// <summary>
    /// Service interface for logging messages to their corresponding category's log file.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an innocent message.
        /// </summary>
        /// <param name="msg">The message.</param>
        void LogMessage(string msg);

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="msg">The warning.</param>
        void LogWarning(string msg);

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="msg">The error.</param>
        void LogError(string msg);
    }
}
