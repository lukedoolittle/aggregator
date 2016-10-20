using System;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
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
            var callbackUri = new Uri("http://localhost:33533/twitter");

            const string consumerKey = "";
            const string consumerSecret = "";

            var request = await new HttpRequestBuilder(baseAddress)
                .PostTo(path, HttpParameterType.Querystring)
                .ForOAuth1RequestToken(
                    consumerKey, 
                    consumerSecret, 
                    callbackUri)
                .ExecuteAsync()
                .ConfigureAwait(false);

        }
    }
}
