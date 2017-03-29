using System;
using Material.Domain.Credentials;
using Material.Application;
using Material.Contracts;
using Material.Framework;
using Material.Domain.Requests;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Interactive.Test.Windows
{
    [Trait("Category", "Manual")]
    public class OAuth2CodeTokenTests
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);
        private readonly TokenCredentialRepository _tokenRepository = 
            new TokenCredentialRepository(true);

        public OAuth2CodeTokenTests()
        {
            Platform.Current.Initialize();
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebookTwice()
        {
            var clientId = _appRepository.GetClientId<Facebook>();
            var clientSecret = _appRepository.GetClientSecret<Facebook>();
            var redirectUri = _appRepository.GetRedirectUri<Facebook>();

            var app = new OAuth2App<Facebook>(
                clientId,
                redirectUri)
                .AddScope<FacebookUser>()
                .AddScope<FacebookEvent>()
                .AddScope<FacebookFeed>()
                .AddScope<FacebookFriend>()
                .AddScope<FacebookPageLike>();

            var token = await app.GetCredentialsAsync(clientSecret).ConfigureAwait(false);
            token = await app.GetCredentialsAsync(clientSecret).ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

            _tokenRepository.PutToken<Facebook, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var clientSecret = _appRepository.GetClientSecret<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var token = await new OAuth2App<Google>(
                        clientId,
                        redirectUri)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .AddScope<GoogleProfile>()
                    .AddScope<GoogleAnalyticsReports>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<FacebookUser>()
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<FitbitIntradayHeartRate>()
                    .AddScope<FitbitIntradayHeartRateBulk>()
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .AddScope<FitbitSleep>()
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<TwentyThreeAndMeUser>()
                    .AddScope<TwentyThreeAndMeGenome>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

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
                        redirectUri)
                    .AddScope<InstagramLikes>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

            _tokenRepository.PutToken<Instagram, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromAmazon()
        {
            var clientId = _appRepository.GetClientId<Amazon>();
            var clientSecret = _appRepository.GetClientSecret<Amazon>();
            var redirectUri = _appRepository.GetRedirectUri<Amazon>();

            var token = await new OAuth2App<Amazon>(
                        clientId,
                        redirectUri)
                    .AddScope<AmazonProfile>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

            _tokenRepository.PutToken<Amazon, OAuth2Credentials>(token);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromPinterest()
        {
            var clientId = _appRepository.GetClientId<Pinterest>();
            var clientSecret = _appRepository.GetClientSecret<Pinterest>();
            var redirectUri = _appRepository.GetRedirectUri<Pinterest>();

            var token = await new OAuth2App<Pinterest>(
                        clientId,
                        redirectUri)
                    .AddScope<PinterestLikes>()
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));

            _tokenRepository.PutToken<Pinterest, OAuth2Credentials>(token);
        }
    }
}
