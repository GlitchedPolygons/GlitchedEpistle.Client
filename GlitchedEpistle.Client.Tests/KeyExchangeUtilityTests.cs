using Xunit;
using System.IO;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;

namespace GlitchedEpistle.Client.Tests
{
    public class KeyExchangeUtilityTests
    {
        private readonly string privateKeyPem = File.ReadAllText("TestData/test.private.rsa");
        private readonly string publicTestKeyPem = File.ReadAllText("TestData/test.public.rsa");

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