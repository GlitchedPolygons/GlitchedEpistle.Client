using Xunit;
using System.IO;

using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric;

namespace GlitchedEpistle.Client.Tests
{
    public class SymmetricCryptographyTests
    {
        private readonly ISymmetricCryptography crypto = new SymmetricCryptography();
        private readonly string privateKeyPem = File.ReadAllText("test.private.rsa");
        private readonly string publicTestKeyPem = File.ReadAllText("test.public.rsa");
        private readonly string text = File.ReadAllText("lorem-ipsum.txt");
        
        private const string ENCRYPTION_PW = "encryption-password_239äöü!!$°§%ç&";
        private const string WRONG_DECRYPTION_PW = "wrong-pw__5956kjnsdjkbä$öüö¨  \n  \t zzEmDkf542";
        
        [Fact]
        public void SymmetricCryptography_EncryptStringUsingPw_DecryptStringUsingPw_IdenticalAfterwards()
        {
            string encr = crypto.EncryptWithPassword(text, ENCRYPTION_PW);
            string decr = crypto.DecryptWithPassword(encr, ENCRYPTION_PW);

            Assert.Equal(text, decr);
        }
        
        [Fact]
        public void SymmetricCryptography_EncryptStringUsingPw_DecryptStringUsingWrongPw_ReturnsNull()
        {
            string encr = crypto.EncryptWithPassword(text, ENCRYPTION_PW);
            string decr = crypto.DecryptWithPassword(encr, WRONG_DECRYPTION_PW);

            Assert.NotEqual(text, decr);
            Assert.Null(decr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SymmetricCryptography_EncryptStringUsingNullOrEmptyPw_ReturnsEmptyString(string pw)
        {
            string encr = crypto.EncryptWithPassword(text, pw);
            Assert.Empty(encr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SymmetricCryptography_EncryptNullOrEmptyString_ReturnsEmptyString(string data)
        {
            string encr = crypto.EncryptWithPassword(data, ENCRYPTION_PW);
            Assert.Empty(encr);
        }
    }
}