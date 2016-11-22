using System;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Extensions;
using Org.BouncyCastle.Crypto.Parameters;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class SignatureVerificationTests
    {
        private readonly IJsonWebTokenSigningFactory _factory =
            new JsonWebTokenSignerFactory();

        private readonly TestData.TestData _testData = new TestData.TestData();

        [Fact]
        public void VerifyRs256JsonWebToken()
        {
            var signatureBase = _testData.Rs256.SignatureBase;
            var signature = _testData.Rs256.Signature;
            var publicKey = _testData.Rs256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var key = new CryptoKey(publicKey, false);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key,
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyEs256JsonWebToken()
        {
            var signatureBase = _testData.Es256.SignatureBase;
            var signature = _testData.Es256.Signature;
            var publicKey = _testData.Es256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var key = new CryptoKey(publicKey, false);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);
            
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key,
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyHs256JsonWebToken()
        {
            var signatureBase = _testData.Hs256.SignatureBase;
            var signature = _testData.Hs256.Signature;
            var hashKey = _testData.Hs256.PrivateKey;
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var key = new HashKey(hashKey);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key,
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyRs256JsonWebTokenWithJsonWebKey()
        {
            var signatureBase = _testData.Rs256.SignatureBase;
            var signature = _testData.Rs256.Signature;
            var publicKey = _testData.Rs256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var cryptoKeyParameters = new CryptoKey(publicKey, false)
                .GetParameter<RsaKeyParameters>();

            var key = new JsonWebKey()
            {
                Algorithm = "RS256",
                KeyType = "RSA",
                E = Convert.ToBase64String(cryptoKeyParameters.Exponent.ToByteArray()),
                N = Convert.ToBase64String(cryptoKeyParameters.Modulus.ToByteArray())
            };

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key.ToCryptoKey(),
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyEs256JsonWebTokenWithJsonWebKey()
        {
            var signatureBase = _testData.Es256.SignatureBase;
            var signature = _testData.Es256.Signature;
            var publicKey = _testData.Es256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var cryptoKeyParameters = new CryptoKey(publicKey, false)
                .GetParameter<ECPublicKeyParameters>();

            var key = new JsonWebKey()
            {
                Algorithm = "ES256",
                KeyType = "EC",
                CurveName = "P-256",
                X = Convert.ToBase64String(cryptoKeyParameters.Q.AffineXCoord.ToBigInteger().ToByteArray()),
                Y = Convert.ToBase64String(cryptoKeyParameters.Q.AffineYCoord.ToBigInteger().ToByteArray())
            };

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key.ToCryptoKey(),
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }
    }
}
