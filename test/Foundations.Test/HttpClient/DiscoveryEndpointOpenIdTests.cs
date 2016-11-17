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
    public class DiscoveryEndpointOpenIdTests
    {
        [Fact]
        public async void PublicKeysFromGoogleOpenIdDiscoveryEndpoint()
        {
            var minKeyCount = 2;
            var algorithms = new List<JsonWebTokenAlgorithm>
            {
                JsonWebTokenAlgorithm.RS256
            };

            var discoveryUrl = new Uri("https://accounts.google.com/.well-known/openid-configuration");

            var response = await new HttpRequestBuilder(discoveryUrl.NonPath())
                .GetFrom(discoveryUrl.AbsolutePath)
                .ResultAsync<OpenIdConnectDiscoveryDocument>()
                .ConfigureAwait(false);

            var keysUrl = new Uri(response.JsonWebKeysUri);

            var keys = await new HttpRequestBuilder(keysUrl.NonPath())
                .GetFrom(keysUrl.AbsolutePath)
                .ResultAsync<PublicKeyDiscoveryDocument>()
                .ConfigureAwait(false);

            Assert.True(keys.Keys.Count >= minKeyCount);

            foreach (var key in keys.Keys)
            {
                Assert.True(algorithms.Contains(
                    key.Algorithm.StringToEnum<JsonWebTokenAlgorithm>()));
            }
        }

        [Fact]
        public async void PublicKeysFromMicrosoftOpenIdDiscoveryEndpoint()
        {
            var minKeyCount = 2;
            var algorithm = "RSA";

            var discoveryUrl = new Uri("https://login.microsoftonline.com/common/.well-known/openid-configuration");

            var response = await new HttpRequestBuilder(discoveryUrl.NonPath())
                .GetFrom(discoveryUrl.AbsolutePath)
                .ResultAsync<OpenIdConnectDiscoveryDocument>()
                .ConfigureAwait(false);

            var keysUrl = new Uri(response.JsonWebKeysUri);

            var keys = await new HttpRequestBuilder(keysUrl.NonPath())
                .GetFrom(keysUrl.AbsolutePath)
                .ResultAsync<PublicKeyDiscoveryDocument>()
                .ConfigureAwait(false);

            Assert.True(keys.Keys.Count >= minKeyCount);

            foreach (var key in keys.Keys)
            {
                Assert.True(algorithm == key.KeyType);
            }
        }

        [Fact]
        public async void PublicKeysFromYahooOpenIdDiscoveryEndpoint()
        {
            var minKeyCount = 2;
            var algorithms = new List<JsonWebTokenAlgorithm>
            {
                JsonWebTokenAlgorithm.RS256,
                JsonWebTokenAlgorithm.ES256
            };

            var discoveryUrl = new Uri("https://login.yahoo.com/.well-known/openid-configuration");

            var response = await new HttpRequestBuilder(discoveryUrl.NonPath())
                .GetFrom(discoveryUrl.AbsolutePath)
                .ResultAsync<OpenIdConnectDiscoveryDocument>()
                .ConfigureAwait(false);

            var keysUrl = new Uri(response.JsonWebKeysUri);

            var keys = await new HttpRequestBuilder(keysUrl.NonPath())
                .GetFrom(keysUrl.AbsolutePath)
                .ResultAsync<PublicKeyDiscoveryDocument>()
                .ConfigureAwait(false);

            Assert.True(keys.Keys.Count >= minKeyCount);

            foreach (var key in keys.Keys)
            {
                Assert.True(algorithms.Contains(
                    key.Algorithm.StringToEnum<JsonWebTokenAlgorithm>()));
            }
        }
    }
}
