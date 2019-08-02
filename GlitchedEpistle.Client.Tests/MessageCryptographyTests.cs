using Xunit;
using System.IO;

using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric;

namespace GlitchedEpistle.Client.Tests
{
    public class MessageCryptographyTests
    {
        private readonly IMessageCryptography crypto = new MessageCryptography(new SymmetricCryptography(), new AsymmetricCryptographyRSA(), new GZipUtility(), new InMemoryLogger());
        
        private readonly string text = File.ReadAllText("TestData/lorem-ipsum.txt");
        private readonly string privateKeyPem = File.ReadAllText("TestData/test.private.rsa");
        private readonly string publicTestKeyPem = File.ReadAllText("TestData/test.public.rsa");
        private readonly byte[] data = new byte[] { 1, 2, 3, 64, 128, 1, 3, 3, 7, 6, 9, 4, 2, 0, 1, 9, 9, 6, 58, 67, 55, 100, 96 };

        private const string ENCRYPTION_PW = "encryption-password_239äöü!!$°§%ç&";
        private const string WRONG_DECRYPTION_PW = "wrong-pw__5956kjnsdjkbä$öüö¨  \n  \t zzEmDkf542";

        [Fact]
        public void MessageCryptography_Encrypt_Decrypt_IdenticalAfterwards()
        {
            string encr = crypto.EncryptMessage(text, publicTestKeyPem);
            string decr = crypto.DecryptMessage(encr, privateKeyPem);
            Assert.Equal(decr, text);
        }
    }
}