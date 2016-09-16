using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuth2ImplicitTokenTests
    {
        private readonly AppCredentialRepository _appRepository;
        private readonly TokenCredentialRepository _tokenRepository;

        public OAuth2ImplicitTokenTests()
        {
            _appRepository = new AppCredentialRepository(CallbackTypeEnum.Localhost);
            _tokenRepository = new TokenCredentialRepository(true);
        }

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

            Assert.True(IsValidToken(token));
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

            Assert.True(IsValidToken(token));
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

            Assert.True(IsValidToken(token));
        }

        [Fact]
        public async void GetAccessTokenFromLinkedInImplicitFlowThrowsException()
        {
            var clientId = _appRepository.GetClientId<LinkedIn>();
            var redirectUri = _appRepository.GetRedirectUri<LinkedIn>();

            await Assert.ThrowsAsync<InvalidFlowTypeException>(() => new OAuth2App<LinkedIn>(
                        clientId,
                        redirectUri)
                .GetCredentialsAsync())
                .ConfigureAwait(false);
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

            Assert.True(IsValidToken(token));
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

            Assert.True(IsValidToken(token));
        }

        [Fact]
        public async void GetAccessTokenFromRunkeeperImplicitFlowThrowsException()
        {
            var clientId = _appRepository.GetClientId<Runkeeper>();
            var redirectUri = _appRepository.GetRedirectUri<Runkeeper>();

            await Assert.ThrowsAsync<InvalidFlowTypeException>(() => new OAuth2App<Runkeeper>(
                        clientId,
                        redirectUri)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync())
                    .ConfigureAwait(false);
        }

        [Fact]
        public async void GetAccessTokenFromRescuetimeImplicitFlowThrowsException()
        {
            var clientId = _appRepository.GetClientId<Rescuetime>();
            var redirectUri = _appRepository.GetRedirectUri<Rescuetime>();

            await Assert.ThrowsAsync<InvalidFlowTypeException>(() => new OAuth2App<Rescuetime>(
                        clientId,
                        redirectUri)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void GetAccessTokenFromPinterestImplicitFlowThrowsException()
        {
            var clientId = _appRepository.GetClientId<Pinterest>();
            var redirectUri = _appRepository.GetRedirectUri<Pinterest>();

            await Assert.ThrowsAsync<InvalidFlowTypeException>(() => new OAuth2App<Pinterest>(
                        clientId,
                        redirectUri)
                    .AddScope<PinterestLikes>()
                    .GetCredentialsAsync())
                .ConfigureAwait(false);
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

            Assert.True(IsValidToken(token));
        }

        private bool IsValidToken(OAuth2Credentials token)
        {
            return token != null &&
                   token.AccessToken != string.Empty &&
                   token.TokenName != string.Empty;
        }
    }
}
