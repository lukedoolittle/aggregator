using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Framework;
using Material.Infrastructure;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.OAuth.Workflow;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Quantfabric.Test.Material.Mocks;
using Quantfabric.Test.Material.OAuthServer;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Handlers;
using Quantfabric.Test.Material.OAuthServer.Tokens;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    [Trait("Category", "Automated")]
    public class MockOAuth2WebTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackType.Localhost);

        public MockOAuth2WebTests()
        {
            HttpConfiguration.MessageHandlerFactory = (parameters) =>
            new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            Platform.Current.Initialize();
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            await RunServer<Google, GoogleMock>(
                    app => app.AddScope<GoogleGmail>()
                        .AddScope<GoogleGmailMetadata>()
                        .AddScope<GoogleProfile>()
                        .AddScope<GoogleAnalyticsReports>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebook()
        {
            await RunServer<Facebook, FacebookMock>(
                    app => app
                            .AddScope<FacebookEvent>()
                            .AddScope<FacebookFriend>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromPinterest()
        {
            await RunServer<Pinterest, PinterestMock>(
                    app => app
                            .AddScope<PinterestLikes>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromInstagram()
        {
            await RunServer<Instagram, InstagramMock>(
                    app => app
                            .AddScope<InstagramLikes>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquare()
        {
            await RunServer<Foursquare, FoursquareMock>(
                app => { },
                false)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotify()
        {
            await RunServer<Spotify, SpotifyMock>(
                    app => app
                            .AddScope<SpotifySavedTrack>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbit()
        {
            await RunServer<Fitbit, FitbitMock>(
                    app => app
                            .AddScope<FitbitIntradayHeartRate>()
                            .AddScope<FitbitIntradayHeartRateBulk>()
                            .AddScope<FitbitIntradaySteps>()
                            .AddScope<FitbitIntradayStepsBulk>()
                            .AddScope<FitbitProfile>()
                            .AddScope<FitbitSleep>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRescuetime()
        {
            await RunServer<Rescuetime, RescuetimeMock>(
                    app => app
                            .AddScope<RescuetimeAnalyticData>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRunkeeper()
        {
            await RunServer<Runkeeper, RunkeeperMock>(
                    app => app
                            .AddScope<RunkeeperFitnessActivity>(),
                false)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTwentyThreeAndMe()
        {
            await RunServer<TwentyThreeAndMe, TwentyThreeAndMeMock>(
                    app => app
                            .AddScope<TwentyThreeAndMeUser>()
                            .AddScope<TwentyThreeAndMeGenome>(),
                    true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromLinkedIn()
        {
            await RunServer<LinkedIn, LinkedInMock>(
                app => { },
                false)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromAmazon()
        {
            await RunServer<Amazon, AmazonMock>(
                    app => app
                            .AddScope<AmazonProfile>(),
                    true)
                .ConfigureAwait(false);
        }

        private async Task RunServer<TRealProvider, TMockProvider>(
            Action<OAuth2Web<TMockProvider>> scopes,
            bool requiresScope)
            where TRealProvider : OAuth2ResourceProvider
            where TMockProvider : OAuth2ResourceProviderMock, new()
        {
            var clientId = _appRepository.GetClientId<TRealProvider>();
            var clientSecret = _appRepository.GetClientSecret<TRealProvider>();

            var port = TestUtilities.GetAvailablePort(35000);
            var providerName = typeof (TRealProvider).Name;
            var redirectUri = new Uri($"http://localhost:{port}/oauth/{providerName}");

            var oauth2 = new OAuth2Web<TMockProvider>(
                clientId, 
                clientSecret, 
                redirectUri.ToString());
            scopes(oauth2);

            var mock = oauth2
                .GetMemberValue<TMockProvider>("_resourceProvider");

            using (var server = new OAuthTestingServer<OAuth2Token>())
            {
                server
                    .AddApplicationId(clientId)
                    .AddHandler(
                        mock.AuthorizationUrl,
                        new OAuth2AuthorizationHandler(
                            clientId,
                            redirectUri,
                            new OAuth2CodeRedirectBuilder(),
                            new OAuth2CodeCredentialBuilder(
                                server.Tokens),
                            requiresScope))
                    .AddHandler(
                        mock.TokenUrl,
                        new OAuth2AccessTokenHandler(
                            clientId,
                            new OAuth2AuthCodeCredentialBuilder(
                                clientSecret,
                                redirectUri,
                                server.Tokens,
                                3600)));

                var serverTask = server.Start(mock.Port);

                var authUri = await oauth2
                    .GetAuthorizationUriAsync()
                    .ConfigureAwait(false);

                if (serverTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.Null(serverTask.Result);
                }

                var handler = new HttpRequestBuilder(authUri.NonPath())
                    .GetFrom(authUri.AbsolutePath)
                    .Parameters(HttpUtility.ParseQueryString(authUri.Query));
                var result = await handler.ExecuteAsync().ConfigureAwait(false);

                if (serverTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.Null(serverTask.Result);
                }

                var tokenTask = oauth2.GetAccessTokenAsync(
                    result.Headers.Location);

                await Task.WhenAny(serverTask, tokenTask)
                    .ConfigureAwait(false);

                if (serverTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.Null(serverTask.Result);
                }

                if (tokenTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.True(TestUtilities.IsValidOAuth2Token(tokenTask.Result));
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
}
