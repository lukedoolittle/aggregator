using System.Text;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class CryptographyTests
    {
        private readonly Randomizer _randomizer =
            new Randomizer();
        private readonly IJsonWebTokenSigningFactory _factory = 
            new JsonWebTokenSignerFactory();

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

        [Fact]
        public void DecomposingGeneratedRsaKeyIntoModulusAndExponent()
        {
            var plainText = _randomizer.RandomString(100);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyPair = RsaCryptoKeyPair.Create(1024);
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var cipherTextBytes = signer.SignText(
                plainTextBytes, 
                keyPair.Private);

            var key = new RsaCryptoKey(
                keyPair.Public.Modulus, 
                keyPair.Public.Exponent);

            var isValid = verifier.VerifyText(
                key, 
                cipherTextBytes, 
                plainTextBytes);

            Assert.True(isValid);
        }

        [Fact]
        public void DecomposingGeneratedEcKeyIntoCurveNameAndCoordinates()
        {
            var curveName = "P-256";
            var plainText = _randomizer.RandomString(100);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var cipherTextBytes = signer.SignText(
                plainTextBytes,
                keyPair.Private);

            var key = new EcdsaCryptoKey(
                keyPair.Public.AlgorithmName,
                keyPair.Public.CurveName,
                keyPair.Public.XCoordinate,
                keyPair.Public.Coordinate);

            var isValid = verifier.VerifyText(
                key,
                cipherTextBytes,
                plainTextBytes);

            Assert.True(isValid);
        }

    }
}
