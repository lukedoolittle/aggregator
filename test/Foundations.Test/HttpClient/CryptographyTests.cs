using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Extensions;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
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
                keyPair.Public.YCoordinate);

            var isValid = verifier.VerifyText(
                key,
                cipherTextBytes,
                plainTextBytes);

            Assert.True(isValid);
        }

        [Fact]
        public void ConvertGoogleDiscoveryUrlKeysIntoCryptoKeys()
        {
            var discoveryUrl = new Uri("https://accounts.google.com/.well-known/openid-configuration");

            var keys = GetKeysFromDiscoveryEntpoint(discoveryUrl);

            foreach (var key in keys)
            {
                var cryptoKey = key.ToCryptoKey();
                Assert.NotNull(cryptoKey);
            }
        }

        [Fact]
        public void ConvertMicrosoftDiscoveryUrlKeysIntoCryptoKeys()
        {
            var discoveryUrl = new Uri("https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration");

            var keys = GetKeysFromDiscoveryEntpoint(discoveryUrl);

            foreach (var key in keys)
            {
                var cryptoKey = key.ToCryptoKey();
                Assert.NotNull(cryptoKey);
            }
        }

        [Fact]
        public void ConvertYahooDiscoveryUrlKeysIntoCryptoKeys()
        {
            var discoveryUrl = new Uri("https://login.yahoo.com/.well-known/openid-configuration");

            var keys = GetKeysFromDiscoveryEntpoint(discoveryUrl);

            var cryptoKeys = keys.Select(k => k.ToCryptoKey()).ToList();

            foreach (var key in cryptoKeys)
            {
                Assert.NotNull(key);
            }

            Assert.True(cryptoKeys.Any(k => k is EcdsaCryptoKey));
        }

        private IList<PublicKeyParameters> GetKeysFromDiscoveryEntpoint(Uri discoveryUrl)
        {
            var discoveryDocument = new HttpRequestBuilder(discoveryUrl.NonPath())
                .GetFrom(discoveryUrl.AbsolutePath)
                .ResultAsync<OpenIdConnectDiscoveryDocument>()
                .Result;

            var keysUrl = new Uri(discoveryDocument.JsonWebKeysUri);

            var keys = new HttpRequestBuilder(keysUrl.NonPath())
                .GetFrom(keysUrl.AbsolutePath)
                .ResultAsync<PublicKeyDiscoveryDocument>()
                .Result;

            return keys.Keys;
        }
    }
}
