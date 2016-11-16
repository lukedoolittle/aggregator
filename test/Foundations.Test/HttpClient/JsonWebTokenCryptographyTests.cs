using System.Text;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class JsonWebTokenCryptographyTests
    {
        private readonly IJsonWebTokenSigningFactory _factory =
            new JsonWebTokenSignerFactory();
        private readonly Randomizer _randomizer =
            new Randomizer();

        [Fact]
        public void CreateAndVerifyRs256JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyRs384JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyRs512JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyPs256JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.PS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyPs384JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.PS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyPs512JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(2048);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.PS512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyEs256JsonWebToken()
        {
            var curveName = "P-256";
            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyEs384JsonWebToken()
        {
            var curveName = "P-256";
            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void CreateAndVerifyEs512JsonWebToken()
        {
            var curveName = "P-256";
            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
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
                new HashKey(key));

            var isVerified = verifier.VerifyText(
                new HashKey(key),
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
                new HashKey(key));

            var isVerified = verifier.VerifyText(
                new HashKey(key),
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
                new HashKey(key));

            var isVerified = verifier.VerifyText(
                new HashKey(key),
                signature,
                bytes);

            Assert.True(isVerified);
        }
    }
}
