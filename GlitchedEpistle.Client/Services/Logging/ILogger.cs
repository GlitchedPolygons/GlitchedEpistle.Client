
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
