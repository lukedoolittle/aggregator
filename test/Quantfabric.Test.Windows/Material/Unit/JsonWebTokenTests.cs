using System;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Extensions;
using Material.Infrastructure.Credentials;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class JsonWebTokenTests
    {
        private readonly Randomizer _randomizer = 
            new Randomizer();
        private readonly IJsonWebTokenSigningFactory _factory = 
            new JsonWebTokenSignerFactory();

        [Fact]
        public void VerifyJsonWebTokenSignatureWithPublicKey()
        {
            var header = new JsonWebTokenHeader
            {
                Algorithm = JsonWebTokenAlgorithm.RS256
            };
            var claims = new JsonWebTokenClaims
            {
                Issuer = _randomizer.RandomString(0, 70),
                Scope = _randomizer.RandomString(0, 50),
                Audience = _randomizer.RandomString(0, 50),
                ExpirationTime = DateTime.Now.AddHours(1),
                IssuedAt = DateTime.Now
            };
            var token = new JsonWebToken(header, claims);

            var signer = _factory.GetSigningAlgorithm(
                token.Header.
                Algorithm);
            var verifier = _factory.GetVerificationAlgorithm(
                token.Header.Algorithm);

            var signatureBase = token.SignatureBase;
            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var keyPair = RsaCryptoKeyPair.Create(1024);

            var signature = signer.SignText(
                signatureBaseBytes,
                keyPair.Private);

            Assert.True(
                verifier.VerifyText(
                    keyPair.Public, 
                    signature,
                    signatureBaseBytes));
        }

        [Fact]
        public void VerifyJsonWebTokenRS256SignatureWithJsonWebKey()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);

            var key = new JsonWebKey()
            {
                Algorithm = "RS256",
                KeyType = "RSA",
                E = Convert.ToBase64String(keyPair.Public.ExponentBytes),
                N = Convert.ToBase64String(keyPair.Public.ModulusBytes)
            };
            var cryptoKey = key.ToCryptoKey();

            var header = new JsonWebTokenHeader
            {
                Algorithm = JsonWebTokenAlgorithm.RS256
            };
            var claims = new JsonWebTokenClaims
            {
                Issuer = _randomizer.RandomString(0, 70),
                Scope = _randomizer.RandomString(0, 50),
                Audience = _randomizer.RandomString(0, 50),
                ExpirationTime = DateTime.Now.AddHours(1),
                IssuedAt = DateTime.Now
            };
            var token = new JsonWebToken(header, claims);

            var signatureBase = token.SignatureBase;
            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signer = _factory.GetSigningAlgorithm(
                token.Header.Algorithm);

            var verifier = _factory.GetVerificationAlgorithm(
                token.Header.Algorithm);

            var signature = signer.SignText(
                signatureBaseBytes,
                keyPair.Private);

            Assert.True(verifier.VerifyText(
                cryptoKey, 
                signature, 
                signatureBaseBytes));
        }

        [Fact]
        public void VerifyJsonWebTokenES256SignatureWithJsonWebKey()
        {
            var keyPair = EcdsaCryptoKeyPair.Create("P-256");

            var key = new JsonWebKey()
            {
                Algorithm = "ES256",
                KeyType = "EC",
                CurveName = "P-256",
                X = Convert.ToBase64String(keyPair.Public.XCoordinateBytes),
                Y = Convert.ToBase64String(keyPair.Public.YCoordinateBytes)
            };
            var cryptoKey = key.ToCryptoKey();

            var header = new JsonWebTokenHeader
            {
                Algorithm = JsonWebTokenAlgorithm.ES256
            };
            var claims = new JsonWebTokenClaims
            {
                Issuer = _randomizer.RandomString(0, 70),
                Scope = _randomizer.RandomString(0, 50),
                Audience = _randomizer.RandomString(0, 50),
                ExpirationTime = DateTime.Now.AddHours(1),
                IssuedAt = DateTime.Now
            };
            var token = new JsonWebToken(header, claims);

            var signatureBase = token.SignatureBase;
            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signer = _factory.GetSigningAlgorithm(
                token.Header.Algorithm);

            var verifier = _factory.GetVerificationAlgorithm(
                token.Header.Algorithm);

            var signature = signer.SignText(
                signatureBaseBytes,
                keyPair.Private);

            Assert.True(verifier.VerifyText(
                cryptoKey,
                signature,
                signatureBaseBytes));
        }
    }
}
