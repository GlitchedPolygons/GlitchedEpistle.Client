using System;
using System.Collections.Generic;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos (local).
    /// </summary>
    public interface IConvoProvider
    {
        /// <summary>
        /// Gets all the convos currently loaded.
        /// </summary>
        /// <value>The convos.</value>
        ICollection<Convo> Convos { get; }

        /// <summary>
        /// Gets the <see cref="Convo"/> with the specified identifier.
        /// </summary>
        /// <param name="id">The <see cref="Convo.Id"/> identifier.</param>
        /// <returns>The matching <see cref="Convo"/> (or <c>null</c> if the convo couldn't be found).</returns>
        Convo this[string id] { get; }

        /// <summary>
        /// Saves all the currently loaded <see cref="Convo"/>s in memory out to disk (or somewhere persistent; depends on the implementation).
        /// </summary>
        void Save();

        /// <summary>
        /// Loads all <see cref="Convo"/>s available on the persistent storage medium (usually local hard drive)
        /// into the <see cref="Convos"/> collection (the existing collection is overwritten!).
        /// </summary>
        void Load();

        /// <summary>
        /// Occurs when the <see cref="IConvoProvider"/> has finished loading/refreshing <see cref="Convo"/>s into the <see cref="Convos"/> collection.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Shall be raised when the <see cref="IConvoProvider"/> has finished
        /// saving the current state of the <see cref="Convos"/> out to persistent memory.
        /// </summary>
        event EventHandler Saved;
    }
}
