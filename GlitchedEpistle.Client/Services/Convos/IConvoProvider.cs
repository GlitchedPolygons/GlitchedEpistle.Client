using System.Collections.Generic;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Service interface responsible for retrieving/accessing convos.
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
    }
}
