using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric
{
    public class SymmetricCryptography : ISymmetricCryptography
    {
        public async Task<string> Encrypt(string plainText, string password, string salt)
        {
            //string encryptedText;
            //using (var input = new MemoryStream(Encoding.UTF8.GetBytes(plainText)))
            //using (var output = new MemoryStream())
            //using (var rfc = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 133769))
            //using (var aes = new AesManaged { Key = rfc.GetBytes(32) })
            //using (var cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
            //{
            //    await input.CopyToAsync(cryptoStream);
            //    encryptedText = Convert.ToBase64String(output.ToArray());
            //}
            //return encryptedText;
            throw new NotImplementedException();
        }

        public async Task<string> Decrypt(string encryptedText, string password, string salt)
        {
            throw new NotImplementedException();
        }
    }
}
