using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class RandomStringGenerationTests
    {
        [Fact]
        public void CallingCreateCrypto16SeveralTimesProducesRandomString()
        {
            var crypto = new CryptoStringGenerator();
            var stringLength = 16;

            var crypto1 = crypto.CreateRandomString(stringLength, CryptoStringType.LowercaseAlphanumeric);
            var crypto2 = crypto.CreateRandomString(stringLength, CryptoStringType.LowercaseAlphanumeric);
            var crypto3 = crypto.CreateRandomString(stringLength, CryptoStringType.LowercaseAlphanumeric);

            Assert.Equal(stringLength, crypto1.Length);
            Assert.Equal(stringLength, crypto2.Length);
            Assert.Equal(stringLength, crypto3.Length);

            Assert.NotEqual(crypto1, crypto2);
            Assert.NotEqual(crypto2, crypto3);
        }

        [Fact]
        public void CallingCreateCrypto32SeveralTimesProducesRandomString()
        {
            var crypto = new CryptoStringGenerator();
            var stringLength = 32;

            var crypto1 = crypto.CreateRandomString(stringLength, CryptoStringType.Base64Alphanumeric);
            var crypto2 = crypto.CreateRandomString(stringLength, CryptoStringType.Base64Alphanumeric);
            var crypto3 = crypto.CreateRandomString(stringLength, CryptoStringType.Base64Alphanumeric);

            Assert.Equal(stringLength, crypto1.Length);
            Assert.Equal(stringLength, crypto2.Length);
            Assert.Equal(stringLength, crypto3.Length);

            Assert.NotEqual(crypto1, crypto2);
            Assert.NotEqual(crypto2, crypto3);
        }
    }
}
