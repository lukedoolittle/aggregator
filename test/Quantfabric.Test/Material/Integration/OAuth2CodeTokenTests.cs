using System;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuth2CodeTokenTests
    {
        private readonly AppCredentialRepository _appRepository;
        private readonly TokenCredentialRepository _tokenRepository;

        public OAuth2CodeTokenTests()
        {
            _appRepository = new AppCredentialRepository(CallbackTypeEnum.Localhost);
            _tokenRepository = new TokenCredentialRepository(true);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var clientSecret = _appRepository.GetClientSecret<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var token = await new OAuth2App<Google>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            //here we need to get any existing refresh token because Google only
            //passes that refresh token back with the first authentication
            try
            {
                var currentToken = _tokenRepository.GetToken<Google, OAuth2Credentials>();
                token.TransferRefreshToken(currentToken.RefreshToken);
                _tokenRepository.PutToken<Google, OAuth2Credentials>(token);
            }
            catch (Exception)
            {
                _tokenRepository.PutToken<Google, OAuth2Credentials>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebook()
        {
            var clientId = _appRepository.GetClientId<Facebook>();
            var clientSecret = _appRepository.GetClientSecret<Facebook>();
            var redirectUri = _appRepository.GetRedirectUri<Facebook>();

            var token = await new OAuth2App<Facebook>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<FacebookUser>()
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Facebook, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquare()
        {
            var clientId = _appRepository.GetClientId<Foursquare>();
            var clientSecret = _appRepository.GetClientSecret<Foursquare>();
            var redirectUri = _appRepository.GetRedirectUri<Foursquare>();

            var token = await new OAuth2App<Foursquare>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Foursquare, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromLinkedIn()
        {
            var clientId = _appRepository.GetClientId<LinkedIn>();
            var clientSecret = _appRepository.GetClientSecret<LinkedIn>();
            var redirectUri = _appRepository.GetRedirectUri<LinkedIn>();

            var token = await new OAuth2App<LinkedIn>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<LinkedIn, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotify()
        {
            var clientId = _appRepository.GetClientId<Spotify>();
            var clientSecret = _appRepository.GetClientSecret<Spotify>();
            var redirectUri = _appRepository.GetRedirectUri<Spotify>();

            var token = await new OAuth2App<Spotify>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Spotify, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbit()
        {
            var clientId = _appRepository.GetClientId<Fitbit>();
            var clientSecret = _appRepository.GetClientSecret<Fitbit>();
            var redirectUri = _appRepository.GetRedirectUri<Fitbit>();

            var token = await new OAuth2App<Fitbit>(
                        clientId,
                        clientSecret,
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

            _tokenRepository.PutToken<Fitbit, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRunkeeper()
        {
            var clientId = _appRepository.GetClientId<Runkeeper>();
            var clientSecret = _appRepository.GetClientSecret<Runkeeper>();
            var redirectUri = _appRepository.GetRedirectUri<Runkeeper>();

            var token = await new OAuth2App<Runkeeper>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Runkeeper, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRescuetime()
        {
            var clientId = _appRepository.GetClientId<Rescuetime>();
            var clientSecret = _appRepository.GetClientSecret<Rescuetime>();
            var redirectUri = _appRepository.GetRedirectUri<Rescuetime>();

            var token = await new OAuth2App<Rescuetime>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Rescuetime, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTwentyThreeAndMe()
        {
            var clientId = _appRepository.GetClientId<TwentyThreeAndMe>();
            var clientSecret = _appRepository.GetClientSecret<TwentyThreeAndMe>();
            var redirectUri = _appRepository.GetRedirectUri<TwentyThreeAndMe>();

            var token = await new OAuth2App<TwentyThreeAndMe>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<TwentyThreeAndMeUser>()
                    .AddScope<TwentyThreeAndMeGenome>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<TwentyThreeAndMe, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromInstagram()
        {
            var clientId = _appRepository.GetClientId<Instagram>();
            var clientSecret = _appRepository.GetClientSecret<Instagram>();
            var redirectUri = _appRepository.GetRedirectUri<Instagram>();

            var token = await new OAuth2App<Instagram>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<InstagramLikes>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Instagram, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromPinterest()
        {
            var clientId = _appRepository.GetClientId<Pinterest>();
            var clientSecret = _appRepository.GetClientSecret<Pinterest>();
            var redirectUri = _appRepository.GetRedirectUri<Pinterest>();

            var token = await new OAuth2App<Pinterest>(
                        clientId,
                        clientSecret,
                        redirectUri)
                    .AddScope<PinterestLikes>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidToken(token));

            _tokenRepository.PutToken<Pinterest, OAuth2Credentials>(token);
        }

        private bool IsValidToken(OAuth2Credentials token)
        {
            return token != null &&
                   token.AccessToken != string.Empty &&
                   token.TokenName != string.Empty;
        }
    }
}
