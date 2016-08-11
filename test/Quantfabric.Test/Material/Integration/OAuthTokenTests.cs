using System;
using Material.Exceptions;
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
            var token = await new OAuth1App<Twitter>(
                        credentials.ConsumerKey, 
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth1Token(token, true));
            //Assert.Equal(1, token.AdditionalParameters.Count);
            //screen_name

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Twitter>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            var credentials = _settings
                .GetClientCredentials<Google, OAuth2Credentials>();
            var token = await new OAuth2App<Google>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .GetCredentialsAsync()
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
                    TestSettings.WriteCredentials<Google>(token);
                }
                catch (Exception)
                {
                    TestSettings.WriteCredentials<Google>(token);
                }
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Google, OAuth2Credentials>();
            var token = await new OAuth2App<Google>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<GoogleGmail>()
                    .AddScope<GoogleGmailMetadata>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebook()
        {
            var credentials = _settings
                .GetClientCredentials<Facebook, OAuth2Credentials>();
            var token = await new OAuth2App<Facebook>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Facebook>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebookImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Facebook, OAuth2Credentials>();
            var token = await new OAuth2App<Facebook>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<FacebookEvent>()
                    .AddScope<FacebookFeed>()
                    .AddScope<FacebookFriend>()
                    .AddScope<FacebookPageLike>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquare()
        {
            var credentials = _settings
                .GetClientCredentials<Foursquare, OAuth2Credentials>();
            var token = await new OAuth2App<Foursquare>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Foursquare>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquareImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Foursquare, OAuth2Credentials>();
            var token = await new OAuth2App<Foursquare>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromLinkedIn()
        {
            var credentials = _settings
                .GetClientCredentials<LinkedIn, OAuth2Credentials>();
            var token = await new OAuth2App<LinkedIn>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<LinkedIn>(token);
            }
        }

        [Fact]
        public async void GetAccessTokenFromLinkedInImplicitFlowThrowsException()
        {
            var credentials = _settings
                .GetClientCredentials<LinkedIn, OAuth2Credentials>();
            await Assert.ThrowsAsync<InvalidGrantTypeException>(() => new OAuth2App<LinkedIn>(
                    credentials.ClientId,
                    credentials.CallbackUrl)
                .GetCredentialsAsync())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotify()
        {
            var credentials = _settings
                .GetClientCredentials<Spotify, OAuth2Credentials>();
            var token = await new OAuth2App<Spotify>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Spotify>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotifyImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Spotify, OAuth2Credentials>();
            var token = await new OAuth2App<Spotify>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<SpotifySavedTrack>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbit()
        {
            var credentials = _settings
                .GetClientCredentials<Fitbit, OAuth2Credentials>();
            var token = await new OAuth2App<Fitbit>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<FitbitIntradayHeartRate>()
                    .AddScope<FitbitIntradayHeartRateBulk>()
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .AddScope<FitbitSleep>()
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Fitbit>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbitImplicitFlow()
        {
            var credentials = _settings
                .GetClientCredentials<Fitbit, OAuth2Credentials>();
            var token = await new OAuth2App<Fitbit>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<FitbitIntradayHeartRate>()
                    .AddScope<FitbitIntradayHeartRateBulk>()
                    .AddScope<FitbitIntradaySteps>()
                    .AddScope<FitbitIntradayStepsBulk>()
                    .AddScope<FitbitSleep>()
                    .AddScope<FitbitProfile>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRunkeeper()
        {
            var credentials = _settings
                .GetClientCredentials<Runkeeper, OAuth2Credentials>();
            var token = await new OAuth2App<Runkeeper>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Runkeeper>(token);
            }
        }

        [Fact]
        public async void GetAccessTokenFromRunkeeperImplicitFlowThrowsException()
        {
            var credentials = _settings
                .GetClientCredentials<Runkeeper, OAuth2Credentials>();
            await Assert.ThrowsAsync<InvalidGrantTypeException>(() => new OAuth2App<Runkeeper>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<RunkeeperFitnessActivity>()
                    .GetCredentialsAsync())
                    .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRescuetime()
        {
            var credentials = _settings
                .GetClientCredentials<Rescuetime, OAuth2Credentials>();
            var token = await new OAuth2App<Rescuetime>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Rescuetime>(token);
            }
        }

        [Fact]
        public async void GetAccessTokenFromRescuetimeImplicitFlowThrowsException()
        {
            var credentials = _settings
                .GetClientCredentials<Rescuetime, OAuth2Credentials>();
            await Assert.ThrowsAsync<InvalidGrantTypeException>(() => new OAuth2App<Rescuetime>(
                        credentials.ClientId,
                        credentials.CallbackUrl)
                    .AddScope<RescuetimeAnalyticData>()
                    .GetCredentialsAsync())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFatsecret()
        {
            var credentials = _settings
                    .GetClientCredentials<Fatsecret, OAuth1Credentials>();
            var token = await new OAuth1App<Fatsecret>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);
            Assert.True(IsValidOAuth1Token(token));

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Fatsecret>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromWithings()
        {
            var credentials = _settings
                .GetClientCredentials<Withings, OAuth1Credentials>();
            var token = await new OAuth1App<Withings>(
                        credentials.ConsumerKey,
                        credentials.ConsumerSecret,
                        credentials.CallbackUrl)
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth1Token(token, true));
            //Assert.Equal(1, token.AdditionalParameters.Count);
            //deviceid

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Withings>(token);
            }
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTwentyThreeAndMe()
        {
            var credentials = _settings
                .GetClientCredentials<TwentyThreeAndMe, OAuth2Credentials>();
            var token = await new OAuth2App<TwentyThreeAndMe>(
                        credentials.ClientId,
                        credentials.ClientSecret,
                        credentials.CallbackUrl)
                    .AddScope<TwentyThreeAndMeUser>()
                    .AddScope<TwentyThreeAndMeGenome>()
                    .GetCredentialsAsync()
                    .ConfigureAwait(false);

            Assert.True(IsValidOAuth2Token(token));
            Assert.Equal(0, token.AdditionalParameters.Count);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<TwentyThreeAndMe>(token);
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
