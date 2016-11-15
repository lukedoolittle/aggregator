using System.Text;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Helpers.Cryptography;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class CryptographyTests
    {
        private readonly IJsonWebTokenSigningFactory _factory = 
            new JsonWebTokenSignerFactory();
        private readonly Randomizer _randomizer = 
            new Randomizer();
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
        public void CreateAndVerifyRs256JsonWebToken()
        {
            var keyPair = RsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes, 
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(), 
                signature, 
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyRs384JsonWebToken()
        {
            var keyPair = RsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyRs512JsonWebToken()
        {
            var keyPair = RsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyPs256JsonWebToken()
        {
            var keyPair = RsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.PS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyPs384JsonWebToken()
        {
            var keyPair = RsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.PS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyPs512JsonWebToken()
        {
            var keyPair = RsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.PS512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyEs256JsonWebToken()
        {
            var keyPair = EcdsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyEs384JsonWebToken()
        {
            var keyPair = EcdsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyEs512JsonWebToken()
        {
            var keyPair = EcdsaCryptoKey.Create();
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private.KeyToString());

            var isVerified = verifier.VerifyText(
                keyPair.Public.KeyToString(),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyHs256JsonWebToken()
        {
            var key = _randomizer.RandomString(200);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                key);

            var isVerified = verifier.VerifyText(
                key,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        public void CreateAndVerifyHs384JsonWebToken()
        {
            var key = _randomizer.RandomString(200);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.HS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                key);

            var isVerified = verifier.VerifyText(
                key,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        public void CreateAndVerifyHs512JsonWebToken()
        {
            var key = _randomizer.RandomString(200);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.HS512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                key);

            var isVerified = verifier.VerifyText(
                key,
                signature,
                bytes);

            Assert.True(isVerified);
        }
    }
}
