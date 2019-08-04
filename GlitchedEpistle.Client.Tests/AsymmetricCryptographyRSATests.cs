using Xunit;
using System;
using System.IO;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Asymmetric;

namespace GlitchedEpistle.Client.Tests
{
    public class AsymmetricCryptographyRSATests
    {
        private readonly IAsymmetricCryptographyRSA crypto = new AsymmetricCryptographyRSA();

        private readonly string text = Guid.NewGuid().ToString("D");
        private readonly string privateKeyPem1 = File.ReadAllText("TestData/KeyPair1/Private");
        private readonly string publicTestKeyPem1 = File.ReadAllText("TestData/KeyPair1/Public");
        private readonly string privateKeyPem2 = File.ReadAllText("TestData/KeyPair2/Private");
        private readonly string publicTestKeyPem2 = File.ReadAllText("TestData/KeyPair2/Public");
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_DecryptString_IdenticalAfterwards()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem1);
            string decr = crypto.Decrypt(encr, privateKeyPem1);
            Assert.Equal(decr, text);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptStringUsingPrivateKey_DecryptString_IdenticalAfterwards_ShouldAlsoWork()
        {
            string encr = crypto.Encrypt(text, privateKeyPem1);
            string decr = crypto.Decrypt(encr, privateKeyPem1);
            Assert.Equal(decr, text);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_NotIdenticalWithOriginal()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem1);
            Assert.NotEqual(encr, text);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_EncryptNullOrEmptyString_ReturnsEmptyString(string s)
        {
            string encr = crypto.Encrypt(s, publicTestKeyPem1);
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
            string decr = crypto.Decrypt(s, publicTestKeyPem1);
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
            string encr = crypto.Encrypt(text, publicTestKeyPem1);
            string decr = crypto.Decrypt(encr, publicTestKeyPem1);
            Assert.NotEqual(text,decr);
            Assert.Null(decr);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptString_DecryptUsingGarbageString_ReturnsNull()
        {
            string encr = crypto.Encrypt(text, publicTestKeyPem1);
            string decr = crypto.Decrypt(encr, "LOL");
            Assert.NotEqual(text,decr);
            Assert.Null(decr);
        }
        
        [Fact]
        public void AsymmetricCryptography_EncryptStringUsingInvalidGarbageString_DecryptionFails_ReturnsNull()
        {
            string encr = crypto.Encrypt(text, "LOL");
            string decr = crypto.Decrypt(encr, publicTestKeyPem1);
            Assert.NotEqual(text,decr);
            Assert.Null(encr);
            Assert.Empty(decr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_SignNullOrEmptyString_ReturnsEmptyString(string txt)
        {
            string sig = crypto.Sign(txt, privateKeyPem1);
            Assert.Empty(sig);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AsymmetricCryptography_SignStringUsingNullOrEmptyKey_ReturnsEmptyString(string key)
        {
            string sig = crypto.Sign(text, key);
            Assert.Empty(sig);
        }
        
        [Fact]
        public void AsymmetricCryptography_SignString_VerifySignature_ReturnsTrue_Succeeds()
        {
            string sig = crypto.Sign(text, privateKeyPem1);
            bool verified = crypto.Verify(text, sig, publicTestKeyPem1);
            Assert.True(verified);
            Assert.NotNull(sig);Assert.NotEmpty(sig);
        }
        
        [Fact]
        public void AsymmetricCryptography_SignStringUsingPublicKey_ReturnsNull()
        {
            string sig = crypto.Sign(text, publicTestKeyPem1);
            Assert.Null(sig);
        }
        
        [Fact]
        public void AsymmetricCryptography_SignString_VerifyUsingPrivateKey_ReturnsFalse()
        {
            string sig = crypto.Sign(text, privateKeyPem1);
            bool verified = crypto.Verify(text, sig, privateKeyPem1);
            Assert.False(verified);
        }
        
        [Fact]
        public void AsymmetricCryptography_SignString_VerifyUsingWrongKey_ReturnsFalse()
        {
            string sig = crypto.Sign(text, privateKeyPem1);
            bool verified = crypto.Verify(text, sig, publicTestKeyPem2);
            Assert.False(verified);
            Assert.NotEmpty(Convert.FromBase64String(sig));
            Assert.True(crypto.Verify(text, sig, publicTestKeyPem1));
        }
    }
}