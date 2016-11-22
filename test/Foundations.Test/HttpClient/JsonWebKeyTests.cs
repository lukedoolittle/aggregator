using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Extensions;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class JsonWebKeyTests
    {
        [Fact]
        public void ConvertGoogleDiscoveryUrlKeysIntoCryptoKeys()
        {
            var minKeyCount = 2;
            var algorithms = new List<JsonWebTokenAlgorithm>
            {
                JsonWebTokenAlgorithm.RS256
            };

            var discoveryUrl = new Uri("https://accounts.google.com/.well-known/openid-configuration");

            var keys = GetKeysFromDiscoveryEntpoint(discoveryUrl);

            Assert.True(keys.Count >= minKeyCount);

            foreach (var key in keys)
            {
                Assert.True(algorithms.Contains(
                    key.Algorithm.StringToEnum<JsonWebTokenAlgorithm>()));

                var cryptoKey = key.ToCryptoKey();
                Assert.NotNull(cryptoKey);
            }
        }

        [Fact]
        public void ConvertMicrosoftDiscoveryUrlKeysIntoCryptoKeys()
        {
            var minKeyCount = 2;
            var keyTypes = new List<string>
            {
                "RSA"
            };

            var discoveryUrl = new Uri("https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration");

            var keys = GetKeysFromDiscoveryEntpoint(discoveryUrl);

            Assert.True(keys.Count >= minKeyCount);

            foreach (var key in keys)
            {
                Assert.True(keyTypes.Contains(
                    key.KeyType));

                var cryptoKey = key.ToCryptoKey();
                Assert.NotNull(cryptoKey);
            }
        }

        [Fact]
        public void ConvertYahooDiscoveryUrlKeysIntoCryptoKeys()
        {
            var minKeyCount = 2;
            var algorithms = new List<JsonWebTokenAlgorithm>
            {
                JsonWebTokenAlgorithm.RS256,
                JsonWebTokenAlgorithm.ES256
            };

            var discoveryUrl = new Uri("https://login.yahoo.com/.well-known/openid-configuration");

            var keys = GetKeysFromDiscoveryEntpoint(discoveryUrl);

            Assert.True(keys.Count >= minKeyCount);

            foreach (var key in keys)
            {
                Assert.True(algorithms.Contains(
                    key.Algorithm.StringToEnum<JsonWebTokenAlgorithm>()));

                var cryptoKey = key.ToCryptoKey();
                Assert.NotNull(cryptoKey);
            }
        }

        private IList<JsonWebKey> GetKeysFromDiscoveryEntpoint(Uri discoveryUrl)
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
