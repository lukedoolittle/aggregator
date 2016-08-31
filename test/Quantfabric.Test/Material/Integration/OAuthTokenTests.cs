using System;
using Material.Exceptions;
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
    public class OAuthTokenTests
    {
        private readonly AppCredentialRepository _appRepository;
        private readonly TokenCredentialRepository _tokenRepository;

        public OAuthTokenTests()
        {
            _appRepository = new AppCredentialRepository(CallbackTypeEnum.Localhost);
            _tokenRepository = new TokenCredentialRepository(true);
        }

        #region OAuth1

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

            Assert.True(IsValidOAuth1Token(token, true));

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

            Assert.True(IsValidOAuth1Token(token));

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

            Assert.True(IsValidOAuth1Token(token, true));

            _tokenRepository.PutToken<Withings, OAuth1Credentials>(token);
        }

        #endregion OAuth1

        #region OAuth2 Code

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

            Assert.True(IsValidOAuth2Token(token));

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
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

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

            Assert.True(IsValidOAuth2Token(token));

            _tokenRepository.PutToken<TwentyThreeAndMe, OAuth2Credentials>(token);
        }

        #endregion OAuth2 Code

        #region OAuth2 Token

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

            Assert.True(IsValidOAuth2Token(token));
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

            Assert.True(IsValidOAuth2Token(token));
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

            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void GetAccessTokenFromLinkedInImplicitFlowThrowsException()
        {
            var clientId = _appRepository.GetClientId<LinkedIn>();
            var redirectUri = _appRepository.GetRedirectUri<LinkedIn>();

            await Assert.ThrowsAsync<InvalidGrantTypeException>(() => new OAuth2App<LinkedIn>(
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

            Assert.True(IsValidOAuth2Token(token));
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

            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void GetAccessTokenFromRunkeeperImplicitFlowThrowsException()
        {
            var clientId = _appRepository.GetClientId<Runkeeper>();
            var redirectUri = _appRepository.GetRedirectUri<Runkeeper>();

            await Assert.ThrowsAsync<InvalidGrantTypeException>(() => new OAuth2App<Runkeeper>(
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

            await Assert.ThrowsAsync<InvalidGrantTypeException>(() => new OAuth2App<Rescuetime>(
                        clientId,
                        redirectUri)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync())
                .ConfigureAwait(false);
        }

        #endregion OAuth2 Token

        private bool IsValidOAuth1Token(
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

        private bool IsValidOAuth2Token(OAuth2Credentials token)
        {
            return token != null &&
                   token.AccessToken != string.Empty &&
                   token.TokenName != string.Empty;
        }
    }
}
