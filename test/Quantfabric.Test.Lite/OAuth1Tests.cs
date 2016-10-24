using System;
using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Lite;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Lite
{
    public class OAuth1Tests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackTypeEnum.Localhost);

        [Fact]
        public async void CanGetValidRequestTokenFromTwitter()
        {
            var provider = new Twitter();
            var consumerKey = _appRepository.GetConsumerKey<Twitter>();
            var consumerSecret = _appRepository.GetConsumerSecret<Twitter>();
            var redirectUri = _appRepository.GetRedirectUri<Twitter>();

            var signingAlgorithm = new DotNetHmacSha1SigningAlgorithm();
            var nonceGenerator = new DotNetStringGenerator();

            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                new Uri(redirectUri),
                signingAlgorithm,
                nonceGenerator);

            var authenticator = new OAuth1RequestToken(
                consumerKey,
                new Uri(redirectUri),
                template,
                signingAlgorithm.SignatureMethod);

            var response = await new HttpRequestBuilder(provider.RequestUrl.NonPath())
                .PostTo(provider.RequestUrl.AbsolutePath)
                .Authenticator(authenticator)
                .ExecuteAsync()
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var credentials = await response
                .ContentAsync<OAuth1Credentials>()
                .ConfigureAwait(false);

            Assert.NotEmpty(credentials.OAuthToken);
            Assert.NotEmpty(credentials.OAuthSecret);
        }
    }
}
