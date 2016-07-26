using System;
using Material;
using Material.Exceptions;
using Aggregator.Configuration;
using Aggregator.Test;
using Application.Configuration;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Xunit;
using GoogleGmailMetadata = Material.Infrastructure.Requests.GoogleGmailMetadata;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuthTokenTests
    {
        private readonly CredentialApplicationSettings _settings;
        
        public OAuthTokenTests()
        {
            _settings = new CredentialApplicationSettings();
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTwitter()
        {
            var credentials = _settings
                .GetClientCredentials<Twitter, OAuth1Credentials>();
            var token = await new OAuth1AppFacade<Twitter>(
                        credentials.ConsumerKey, 
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth1Token(token, true));
            Assert.Equal(1, token.AdditionalParameters.Count);
            //screen_name

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Twitter>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            var credentials = _settings
                .GetClientCredentials<Google, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Google>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                //here we need to get any existing refresh token because Google only
                //passes that refresh token back with the first authentication
                try
                {
                    var currentToken = TestSettings.GetToken<Google, OAuth2Credentials>();
                    token.TransferRefreshToken(currentToken.RefreshToken);
                    TestHelpers.WriteCredentials<Google>(token);
                }
                catch (Exception)
                {
                    TestHelpers.WriteCredentials<Google>(token);
                }
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Google, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Google>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebook()
        {
            var credentials = _settings
                .GetClientCredentials<Facebook, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Facebook>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebookImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Facebook, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Facebook>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquare()
        {
            var credentials = _settings
                .GetClientCredentials<Foursquare, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Foursquare>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Foursquare>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquareImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Foursquare, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Foursquare>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromLinkedIn()
        {
            var credentials = _settings
                .GetClientCredentials<LinkedIn, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<LinkedIn>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<LinkedIn>(token);
            }
        }

        [Fact]
        public async void GetAccessTokenFromLinkedInImplicitFlowThrowsException()
        {
            var credentials = _settings
                .GetClientCredentials<LinkedIn, OAuth2Credentials>();
            await Assert.ThrowsAsync<GrantTypeException>(() => new OAuth2AppFacade<LinkedIn>(
                    credentials.ClientId,
                    credentials.CallbackUrl)
                .GetOAuth2Credentials())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotify()
        {
            var credentials = _settings
                .GetClientCredentials<Spotify, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Spotify>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<SpotifySavedTrack>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Spotify>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotifyImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Spotify, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Spotify>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<SpotifySavedTrack>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbit()
        {
            var credentials = _settings
                .GetClientCredentials<Fitbit, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Fitbit>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FitbitIntradayHeartRate>()
                    .AddScope<FitbitIntradayHeartRateBulk>()
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .AddScope<FitbitSleep>()
                    .AddScope<FitbitProfile>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Fitbit>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbitImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Fitbit, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Fitbit>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<FitbitIntradayHeartRate>()
                    .AddScope<FitbitIntradayHeartRateBulk>()
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .AddScope<FitbitSleep>()
                    .AddScope<FitbitProfile>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRunkeeper()
        {
            var credentials = _settings
                .GetClientCredentials<Runkeeper, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Runkeeper>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Runkeeper>(token);
            }
        }

        [Fact]
        public async void GetAccessTokenFromRunkeeperImplicitFlowThrowsException()
        {
            var credentials = _settings
                .GetClientCredentials<Runkeeper, OAuth2Credentials>();
            await Assert.ThrowsAsync<GrantTypeException>(() => new OAuth2AppFacade<Runkeeper>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetOAuth2Credentials())
                    .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRescuetime()
        {
            var credentials = _settings
                .GetClientCredentials<Rescuetime, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<Rescuetime>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Rescuetime>(token);
            }
        }

        [Fact]
        public async void GetAccessTokenFromRescuetimeImplicitFlowThrowsException()
        {
            var credentials = _settings
                .GetClientCredentials<Rescuetime, OAuth2Credentials>();
            await Assert.ThrowsAsync<GrantTypeException>(() => new OAuth2AppFacade<Rescuetime>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetOAuth2Credentials())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFatsecret()
        {
            var credentials = _settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>();
            var token = await new OAuth1AppFacade<Fatsecret>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth1Token(token));

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Fatsecret>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromWithings()
        {
            var credentials = _settings
                .GetClientCredentials<Withings, OAuth1Credentials>();
            var token = await new OAuth1AppFacade<Withings>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetOAuth1Credentials()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth1Token(token, true));
            Assert.Equal(1, token.AdditionalParameters.Count);
            //deviceid

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<Withings>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTwentyThreeAndMe()
        {
            var credentials = _settings
                .GetClientCredentials<TwentyThreeAndMe, OAuth2Credentials>();
            var token = await new OAuth2AppFacade<TwentyThreeAndMe>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<TwentyThreeAndMeUser>()
                    .AddScope<TwentyThreeAndMeGenome>()
                    .GetOAuth2Credentials()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestHelpers.WriteCredentials<TwentyThreeAndMe>(token);
            }
        }

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
