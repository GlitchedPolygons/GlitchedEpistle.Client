using System;
using System.Collections.Generic;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Logging
{
    /// <summary>
    /// <see cref="ILogger"/> implementation that keeps its logs stored in memory. Excellent for unit testing purposes.
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
