using System;

using Xunit;
using System.IO;

using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Symmetric;

namespace GlitchedEpistle.Client.Tests
{
    public class SymmetricCryptographyTests
    {
        private readonly ISymmetricCryptography crypto = new SymmetricCryptography();
        private readonly string privateKeyPem = File.ReadAllText("TestData/KeyPair1/Private");
        private readonly string publicTestKeyPem = File.ReadAllText("TestData/KeyPair1/Public");
        private readonly string text = File.ReadAllText("TestData/LoremIpsum.txt");
        private readonly byte[] data = new byte[] { 1, 2, 3, 64, 128, 1, 3, 3, 7, 6, 9, 4, 2, 0, 1, 9, 9, 6, 58, 67, 55, 100, 96 };
        
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
        public void SymmetricCryptography_EncryptStringUsingPw_NotIdenticalWithOriginal()
        {
            string encr = crypto.EncryptWithPassword(text, ENCRYPTION_PW);
            Assert.NotEqual(encr, text);
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
        
        [Fact]
        public void SymmetricCryptography_EncryptBytesUsingPw_DecryptBytesUsingPw_IdenticalAfterwards()
        {
            byte[] encr = crypto.EncryptWithPassword(data, ENCRYPTION_PW);
            byte[] decr = crypto.DecryptWithPassword(encr, ENCRYPTION_PW);

            Assert.Equal(data, decr);
        }
        
        [Fact]
        public void SymmetricCryptography_EncryptBytesUsingPw_NotIdenticalWithOriginal()
        {
            byte[] encr = crypto.EncryptWithPassword(data, ENCRYPTION_PW);
            Assert.NotEqual(encr, data);
        }
        
        [Fact]
        public void SymmetricCryptography_EncryptBytesUsingPw_DecryptBytesUsingWrongPw_ReturnsNull()
        {
            byte[] encr = crypto.EncryptWithPassword(data, ENCRYPTION_PW);
            byte[] decr = crypto.DecryptWithPassword(encr, WRONG_DECRYPTION_PW);

            Assert.NotEqual(encr, data);
            Assert.Null(decr);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SymmetricCryptography_EncryptBytesUsingNullOrEmptyPw_ReturnsEmptyBytesArray(string pw)
        {
            byte[] encr = crypto.EncryptWithPassword(data, pw);
            Assert.Empty(encr);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        public void SymmetricCryptography_EncryptNullOrEmptyBytes_ReturnsEmptyBytesArray(byte[] data)
        {
            byte[] encr = crypto.EncryptWithPassword(data, ENCRYPTION_PW);
            Assert.Empty(encr);
        }

        [Fact]
        public void SymmetricCryptography_Encrypt_Decrypt_IdenticalAfterwards()
        {
            EncryptionResult encr = crypto.Encrypt(data);
            byte[] decr = crypto.Decrypt(encr);
            Assert.Equal(decr, data);
        }
        
        [Fact]
        public void SymmetricCryptography_Encrypt_DifferentThanOriginal()
        {
            EncryptionResult encr = crypto.Encrypt(data);
            Assert.NotEqual(encr.EncryptedData, data);
        }
        
        [Fact]
        public void SymmetricCryptography_DecryptUsingNull_ReturnsEmptyBytesArray()
        {
            byte[] decr = crypto.Decrypt(null);
            Assert.Empty(decr);
        }
        
        [Fact]
        public void SymmetricCryptography_DecryptEmptyInstance_ReturnsEmptyByteArray()
        {
            byte[] decr = crypto.Decrypt(EncryptionResult.Empty);
            Assert.Empty(decr);
        }
        
        [Fact]
        public void SymmetricCryptography_Encrypt_DecryptUsingWrongData_ReturnsEmptyBytesArray()
        {
            EncryptionResult encr = crypto.Encrypt(data);
            byte[] decr = crypto.Decrypt(new EncryptionResult()
            {
                IV = new byte[] { 4, 5, 6 }, 
                Key = new byte[] { 1, 2, 3 }, 
                EncryptedData = new byte[] { 7, 8, 9 }
            });
            Assert.False(encr.IsEmpty());
            Assert.NotEqual(decr, data);
            Assert.Null(decr);
        }
    }
}