using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    [Trait("Category", "Manual")]
    public class OAuth1TokenTests
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);
        private readonly TokenCredentialRepository _tokenRepository = 
            new TokenCredentialRepository(true);

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

            Assert.True(TestUtilities.IsValidOAuth1Token(token, true));

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

            Assert.True(TestUtilities.IsValidOAuth1Token(token, false));

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

            Assert.True(TestUtilities.IsValidOAuth1Token(token, true));

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

            Assert.True(TestUtilities.IsValidOAuth1Token(token, false));

            _tokenRepository.PutToken<Tumblr, OAuth1Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromAppFigures()
        {
            var consumerKey = _appRepository.GetConsumerKey<AppFigures>();
            var consumerSecret = _appRepository.GetConsumerSecret<AppFigures>();
            var redirectUri = _appRepository.GetRedirectUri<AppFigures>();

            var token = await new OAuth1App<AppFigures>(
                        consumerKey,
                        consumerSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth1Token(token, false));

            _tokenRepository.PutToken<AppFigures, OAuth1Credentials>(token);
        }
    }
}
