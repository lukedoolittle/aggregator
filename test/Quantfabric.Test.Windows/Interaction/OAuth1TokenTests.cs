using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuth1TokenTests
    {
        private readonly AppCredentialRepository _appRepository;
        private readonly TokenCredentialRepository _tokenRepository;

        public OAuth1TokenTests()
        {
            _appRepository = new AppCredentialRepository(CallbackTypeEnum.Localhost);
            _tokenRepository = new TokenCredentialRepository(true);
        }



        [Fact]
        public async void CanGetValidAccessTokenFromTwitter()
        {
            var consumerKey = _appRepository.GetConsumerKey<Twitter>();
            var consumerSecret = _appRepository.GetConsumerSecret<Twitter>();
            var redirectUri = _appRepository.GetRedirectUri<Twitter>();

            var token = await new OAuth1App<Twitter>(
                        consumerKey,
                        consumerSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token, true));

            _tokenRepository.PutToken<Twitter, OAuth1Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFatsecret()
        {
            var consumerKey = _appRepository.GetConsumerKey<Fatsecret>();
            var consumerSecret = _appRepository.GetConsumerSecret<Fatsecret>();
            var redirectUri = _appRepository.GetRedirectUri<Fatsecret>();

            var token = await new OAuth1App<Fatsecret>(
                        consumerKey,
                        consumerSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Fatsecret, OAuth1Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromWithings()
        {
            var consumerKey = _appRepository.GetConsumerKey<Withings>();
            var consumerSecret = _appRepository.GetConsumerSecret<Withings>();
            var redirectUri = _appRepository.GetRedirectUri<Withings>();

            var token = await new OAuth1App<Withings>(
                        consumerKey,
                        consumerSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token, true));

            _tokenRepository.PutToken<Withings, OAuth1Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTumblr()
        {
            var consumerKey = _appRepository.GetConsumerKey<Tumblr>();
            var consumerSecret = _appRepository.GetConsumerSecret<Tumblr>();
            var redirectUri = _appRepository.GetRedirectUri<Tumblr>();

            var token = await new OAuth1App<Tumblr>(
                        consumerKey,
                        consumerSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token, false));

            _tokenRepository.PutToken<Tumblr, OAuth1Credentials>(token);
        }

        private bool IsValidToken(
            OAuth1Credentials token,
            bool shouldContainUserId = false)
        {
            if (shouldContainUserId)
            {
                Assert.NotNull(token.UserId);
                Assert.NotEmpty(token.UserId);
            }
            return token != null &&
                   token.ConsumerKey != string.Empty &&
                   token.ConsumerSecret != string.Empty &&
                   token.OAuthToken != string.Empty &&
                   token.OAuthSecret != string.Empty;
        }
    }
}
