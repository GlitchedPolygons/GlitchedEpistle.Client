using System.Threading.Tasks;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// Asymmetric crypto key generator.
    /// </summary>
    public interface IAsymmetricKeygen
    {
        /// <summary>
        /// Generates the key pair.
        /// </summary>
        /// <param name="outputDirectory">The output directory (where the keys should be exported into).</param>
        /// <returns>Whether the key generation was successful or not.</returns>
        Task<bool> GenerateKeyPair(string outputDirectory);
    }
}
