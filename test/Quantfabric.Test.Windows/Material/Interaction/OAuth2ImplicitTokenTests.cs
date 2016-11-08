using Material.Contracts;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    [Trait("Category", "Manual")]
    public class OAuth2ImplicitTokenTests
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var token = await new OAuth2App<Google>(
                        clientId,
                        redirectUri)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebookImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Facebook>();
            var redirectUri = _appRepository.GetRedirectUri<Facebook>();

            var token = await new OAuth2App<Facebook>(
                        clientId,
                        redirectUri)
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquareImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Foursquare>();
            var redirectUri = _appRepository.GetRedirectUri<Foursquare>();

            var token = await new OAuth2App<Foursquare>(
                        clientId,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotifyImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Spotify>();
            var redirectUri = _appRepository.GetRedirectUri<Spotify>();

            var token = await new OAuth2App<Spotify>(
                        clientId,
                        redirectUri)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbitImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Fitbit>();
            var redirectUri = _appRepository.GetRedirectUri<Fitbit>();

            var token = await new OAuth2App<Fitbit>(
                        clientId,
                        redirectUri)
                    .AddScope<FitbitIntradayHeartRate>()
                    .AddScope<FitbitIntradayHeartRateBulk>()
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .AddScope<FitbitSleep>()
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromAmazonImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Amazon>();
            var redirectUri = _appRepository.GetRedirectUri<Amazon>();

            var token = await new OAuth2App<Amazon>(
                        clientId,
                        redirectUri)
                    .AddScope<AmazonProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromInstagramImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Instagram>();
            var redirectUri = _appRepository.GetRedirectUri<Instagram>();

            var token = await new OAuth2App<Instagram>(
                        clientId,
                        redirectUri)
                    .AddScope<InstagramLikes>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }
    }
}
