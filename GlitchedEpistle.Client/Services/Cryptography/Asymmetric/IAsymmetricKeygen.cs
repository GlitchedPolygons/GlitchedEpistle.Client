﻿#region
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric
{
    /// <summary>
    /// Asymmetric crypto key generator.
    /// </summary>
    public interface IAsymmetricKeygen
    {
        /// <summary>
        /// Generates a new key pair.
        /// </summary>
        /// <returns>The keypair <see cref="Tuple"/>, where the first item is the public RSA key and the second is the private key (both PEM-formatted).</returns>
        Task<Tuple<string, string>> GenerateKeyPair();
    }
}
