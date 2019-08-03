using Xunit;
using System;
using System.IO;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric;

namespace GlitchedEpistle.Client.Tests
{
    public class AsymmetricCryptographyRSATests
    {
        private readonly IAsymmetricCryptographyRSA crypto = new AsymmetricCryptographyRSA();

        private readonly string text = Guid.NewGuid().ToString("D");
        private readonly string privateKeyPem = File.ReadAllText("TestData/test.private.rsa");
        private readonly string publicTestKeyPem = File.ReadAllText("TestData/test.public.rsa");
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_DecryptString_IdenticalAfterwards()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem);
            string decr = crypto.Decrypt(encr, privateKeyPem);
            Assert.Equal(decr, text);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptStringUsingPrivateKey_DecryptString_IdenticalAfterwards_ShouldAlsoWork()
        {
            string encr = crypto.Encrypt(text, privateKeyPem);
            string decr = crypto.Decrypt(encr, privateKeyPem);
            Assert.Equal(decr, text);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_NotIdenticalWithOriginal()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem);
            Assert.NotEqual(encr, text);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_EncryptNullOrEmptyString_ReturnsEmptyString(string s)
        {
            string encr = crypto.Encrypt(s, publicTestKeyPem);
            Assert.Empty(encr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_EncryptStringWithNullOrEmptyKey_ReturnsEmptyString(string s)
        {
            string encr = crypto.Encrypt(text, s);
            Assert.Empty(encr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_DecryptNullOrEmptyString_ReturnsEmptyString(string s)
        {
            string decr = crypto.Decrypt(s, publicTestKeyPem);
            Assert.Empty(decr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_DecryptStringWithNullOrEmptyKey_ReturnsEmptyString(string s)
        {
            string decr = crypto.Decrypt(text, s);
            Assert.Empty(decr);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_DecryptStringUsingPublicKey_ReturnsNull()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem);
            string decr = crypto.Decrypt(encr, publicTestKeyPem);
            Assert.NotEqual(text,decr);
            Assert.Null(decr);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_DecryptUsingGarbageString_ReturnsNull()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem);
            string decr = crypto.Decrypt(encr, "LOL");
            Assert.NotEqual(text,decr);
            Assert.Null(decr);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptStringUsingInvalidGarbageString_DecryptionFails_ReturnsNull()
        {
            string encr = crypto.Encrypt(text, "LOL");
            string decr = crypto.Decrypt(encr, publicTestKeyPem);
            Assert.NotEqual(text,decr);
            Assert.Null(encr);
            Assert.Empty(decr);
        }
    }
}