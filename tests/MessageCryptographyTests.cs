/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using Xunit;
using System.IO;

using GlitchedPolygons.Services.CompressionUtility;
using GlitchedPolygons.Services.Cryptography.Symmetric;
using GlitchedPolygons.Services.Cryptography.Asymmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Logging;
using GlitchedPolygons.GlitchedEpistle.Client.Services.Cryptography.Messages;

namespace GlitchedEpistle.Client.Tests
{
    public class MessageCryptographyTests
    {
        private readonly IMessageCryptography crypto = new MessageCryptography(new SymmetricCryptography(), new AsymmetricCryptographyRSA(), new LzmaUtility(), new LzmaUtilityAsync(), new InMemoryLogger());
        
        private readonly string text = File.ReadAllText("TestData/LoremIpsum.txt");
        private readonly string privateKeyPem = File.ReadAllText("TestData/KeyPair1/Private");
        private readonly string publicTestKeyPem = File.ReadAllText("TestData/KeyPair1/Public");

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