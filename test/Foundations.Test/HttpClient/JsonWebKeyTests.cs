using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Keys;
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
