﻿using System.Threading.Tasks;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.ServerHealth
{
    /// <summary>
    /// Service interface for testing the connection to the epistle server.
    /// </summary>
    public interface IServerConnectionTest
    {
        /// <summary>
        /// Tests the connection to the epistle server.<para> </para>
        /// Returns <c>true</c> if the connection could be established or <c>false</c> if the server did not respond.
        /// </summary>
        /// <returns>Whether the connection to the epistle server could be established successfully or not.</returns>
        Task<bool> TestConnection();
    }
}