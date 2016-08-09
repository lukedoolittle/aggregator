using System;
using Foundations.HttpClient;
using Foundations.HttpClient.Extensions;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    public class OAuthSigningTests
    {
        [Fact]
        public async void GenerateRequestTokenSignature()
        {
            var baseAddress = new Uri("https://api.twitter.com");
            var path = "oauth/request_token";
            var callbackUri = "http://localhost:33533/twitter";

            const string consumerKey = "";
            const string consumerSecret = "";

            var request = await new HttpRequest(baseAddress)
                .PostTo(path)
                .WithQueryParameters()
                .ForOAuth1RequestToken(
                    consumerKey, 
                    consumerSecret, 
                    callbackUri)
                .ExecuteAsync()
                .ConfigureAwait(false);

        }
    }
}
