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

        [Fact]
        public void MessageCryptography_Encrypt_Decrypt_IdenticalAfterwards()
        {
            string encr = crypto.EncryptMessage(text, publicTestKeyPem);
            string decr = crypto.DecryptMessage(encr, privateKeyPem);
            Assert.Equal(decr, text);
        }
        
        [Fact]
        public void MessageCryptography_Encrypt_NotIdenticalWithOriginal()
        {
            string encr = crypto.EncryptMessage(text, publicTestKeyPem);
            Assert.NotEqual(encr, text);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MessageCryptography_EncryptNullOrEmptyString_ReturnsEmptyString(string s)
        {
            string encr = crypto.EncryptMessage(s, publicTestKeyPem);
            Assert.Empty(encr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MessageCryptography_EncryptWithNullOrEmptyKey_ReturnsEmptyString(string s)
        {
            string encr = crypto.EncryptMessage(text, s);
            Assert.Empty(encr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MessageCryptography_DecryptNullOrEmptyString_ReturnsEmptyString(string s)
        {
            string decr = crypto.DecryptMessage(s, publicTestKeyPem);
            Assert.Empty(decr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MessageCryptography_DecryptWithNullOrEmptyKey_ReturnsEmptyString(string s)
        {
            string decr = crypto.DecryptMessage(text, s);
            Assert.Empty(decr);
        }
        
        [Fact]
        public void MessageCryptography_Encrypt_DecryptUsingPublicKey_ReturnsNull()
        {
            string encr = crypto.EncryptMessage(text, publicTestKeyPem);
            string decr = crypto.DecryptMessage(encr, publicTestKeyPem);
            Assert.NotEqual(text,decr);
            Assert.Null(decr);
        }
        
        [Fact]
        public void MessageCryptography_EncryptUsingPrivateKey_Decrypt_IdenticalAfterwards_ShouldAlsoWork()
        {
            string encr = crypto.EncryptMessage(text, privateKeyPem);
            string decr = crypto.DecryptMessage(encr, privateKeyPem);
            Assert.Equal(decr, text);
        }
    }
}