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

using System;
using System.Collections.Generic;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Logging
{
    /// <summary>
    /// <see cref="ILogger"/> implementation that keeps its logs stored in memory.<para> </para>
    /// Excellent for unit testing and debugging purposes.
    /// </summary>
    public class InMemoryLogger : ILogger
    {
        private readonly List<string> messages = new List<string>(64);
        private readonly List<string> warnings = new List<string>(64);
        private readonly List<string> errors = new List<string>(64);
        
        private object messageLock = new object();
        private object warningLock = new object();
        private object errorLock = new object();
        
        private static string Timestamp(string msg)
        {
            return $"[{DateTime.Now.ToString("s")}] {msg}\n";
        }
        
        private static ICollection<string> GetCollection(ICollection<string> input)
        {
            string[] array = new string[input.Count];
            input.CopyTo(array, 0);
            return array;
        } 
            
        /// <summary>
        /// Gets a copy of the currently logged messages.
        /// </summary>
        /// <returns>A new <see cref="ICollection{T}"/> that is a copy of the currently logged messages.</returns>
        public ICollection<string> GetMessages()
        {
            lock (messageLock)
            {
                return GetCollection(messages);
            }
        }
        
        /// <summary>
        /// Gets a copy of the currently logged warnings.
        /// </summary>
        /// <returns>A new <see cref="ICollection{T}"/> that is a copy of the currently logged warnings.</returns>
        public ICollection<string> GetWarnings()
        {
            lock (warningLock)
            {
                return GetCollection(warnings);
            }
        }
        
        /// <summary>
        /// Gets a copy of the currently logged errors.
        /// </summary>
        /// <returns>A new <see cref="ICollection{T}"/> that is a copy of the currently logged errors.</returns>
        public ICollection<string> GetErrors()
        {
            lock (errorLock)
            {
                return GetCollection(errors);
            }
        }

        /// <summary>
        /// Logs an innocent message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public void LogMessage(string msg)
        {
            lock (messageLock)
            {
                messages.Add(Timestamp(msg));
            }
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="msg">The warning.</param>
        public void LogWarning(string msg)
        {
            lock (warningLock)
            {
                warnings.Add(Timestamp(msg));
            }
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="msg">The error.</param>
        public void LogError(string msg)
        {
            lock (errorLock)
            {
                errors.Add(Timestamp(msg));
            }
        }
    }
}
