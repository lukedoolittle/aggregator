using System;
using System.Text;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Org.BouncyCastle.Security;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class JsonWebTokenTests
    {
        private readonly Randomizer _randomizer = new Randomizer();
        private readonly IJsonWebTokenSigningFactory _factory = new JsonWebTokenSignerFactory();

        [Fact]
        public void CreateJsonWebTokenThenVerifySignatureWithPublicKey()
        {
            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = _randomizer.RandomString(0, 70),
                    Scope = _randomizer.RandomString(0, 50),
                    Audience = _randomizer.RandomString(0, 50),
                    ExpirationTime = DateTime.Now.AddHours(1),
                    IssuedAt = DateTime.Now
                }
            };

            var signer = _factory.GetSigningAlgorithm(token.Header.Algorithm);
            var verifier = _factory.GetVerificationAlgorithm(token.Header.Algorithm);

            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signingTemplate = new JsonWebTokenSigningTemplate(
                new JsonWebTokenSignerFactory());
            var signatureBase = signingTemplate.CreateSignatureBase(header, claims);

            var encodedSignatureBase = Encoding.UTF8.GetBytes(signatureBase);

            var keyPair = RsaCryptoKeyPair.Create(1024);

            var cipherText = signer.SignText(
                encodedSignatureBase,
                keyPair.Private);

            Assert.True(
                verifier.VerifyText(
                    keyPair.Public, 
                    cipherText,
                    encodedSignatureBase));
        }

        [Fact]
        public void CreateJsonWebTokenThenVerifySignatureWithModulusAndPublicExponent()
        {
            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = _randomizer.RandomString(0, 70),
                    Scope = _randomizer.RandomString(0, 50),
                    Audience = _randomizer.RandomString(0, 50),
                    ExpirationTime = DateTime.Now.AddHours(1),
                    IssuedAt = DateTime.Now
                }
            };
            
            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signingTemplate = new JsonWebTokenSigningTemplate(
                new JsonWebTokenSignerFactory());
            var signatureBase = signingTemplate.CreateSignatureBase(header, claims);

            var encodedSignatureBase = Encoding.UTF8.GetBytes(signatureBase);

            var keyPair = RsaCryptoKeyPair.Create(1024);

            var factory = new JsonWebTokenSignerFactory();
            var verifier = GetSignatureVerificationAlgorithm(token.Header.Algorithm);
            var signer = factory.GetSigningAlgorithm(token.Header.Algorithm);

            var cipherText = signer.SignText(
                encodedSignatureBase,
                keyPair.Private);

            Assert.True(verifier.VerifyText(
                keyPair.Public.Modulus,
                keyPair.Public.Exponent, 
                cipherText, 
                encodedSignatureBase));
        }

        private static ISignatureVerificationAlgorithm GetSignatureVerificationAlgorithm(
            JsonWebTokenAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case JsonWebTokenAlgorithm.RS256:
                    return new RSASignatureVerificationAlgorithm(SignerUtilities.GetSigner("SHA-256withRSA"));
                case JsonWebTokenAlgorithm.RS384:
                    return new RSASignatureVerificationAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JsonWebTokenAlgorithm.RS512:
                    return new RSASignatureVerificationAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
