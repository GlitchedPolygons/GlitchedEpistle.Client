using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    public interface ISymmetricCryptography
    {
        Task<string> Encrypt(string plainText, string password, string salt); // TODO: wip
        Task<string> Decrypt(string encryptedText, string password, string salt);
    }
}
