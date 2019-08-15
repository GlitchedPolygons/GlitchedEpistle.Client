/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

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
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;

namespace GlitchedEpistle.Client.Tests
{
    public class KeyExchangeUtilityTests
    {
        private readonly string privateKeyPem = File.ReadAllText("TestData/KeyPair1/Private");
        private readonly string publicTestKeyPem = File.ReadAllText("TestData/KeyPair1/Public");

        [Fact]
        public void KeyExchangeUtility_Compress_Decompress_IdenticalAfterwards()
        {
            string compressedKey = KeyExchangeUtility.CompressPublicKey(publicTestKeyPem);
            string decompressedKey = KeyExchangeUtility.DecompressPublicKey(compressedKey);
            Assert.Equal(publicTestKeyPem, decompressedKey);
        }

        [Fact]
        public void KeyExchangeUtility_EncryptAndCompress_DecompressAndDecrypt_IdenticalAfterwards()
        {
            const string TEST_PW = "test.@#°§çUserPassword$$$___69420  \r\n847KWdHfhoö\nüä!\t!]]   [}  \r\n\r\n $äö\"";
            string i = KeyExchangeUtility.EncryptAndCompressPrivateKey(privateKeyPem, TEST_PW);
            string o = KeyExchangeUtility.DecompressAndDecryptPrivateKey(i, TEST_PW);
            Assert.Equal(privateKeyPem, o);
        }
    }
}