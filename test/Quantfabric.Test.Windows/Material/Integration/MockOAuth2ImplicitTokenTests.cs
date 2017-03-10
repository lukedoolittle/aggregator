using System;
using System.Threading.Tasks;
using Material.Application;
using Material.Contracts;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.OAuth;
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
    public class MockOAuth2ImplicitTokenTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackType.Localhost);

        public MockOAuth2ImplicitTokenTests()
        {
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
        public async void CanGetValidAccessTokenFromInstagram()
        {
            await RunServer<Instagram, InstagramMock>(
                    app => app
                            .AddScope<InstagramLikes>(),
                    true)
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
        public async void GetAccessTokenFromAmazonImplicitFlow()
        {
            await RunServer<Amazon, AmazonMock>(
                    app => app
                            .AddScope<AmazonProfile>(),
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

        private async Task RunServer<TRealProvider, TMockProvider>(
            Action<OAuth2App<TMockProvider>> scopes,
            bool requiresScope)
            where TRealProvider : OAuth2ResourceProvider
            where TMockProvider : OAuth2ResourceProviderMock, new()
        {
            var clientId = _appRepository.GetClientId<TRealProvider>();

            var port = TestUtilities.GetAvailablePort(35000);
            var providerName = typeof(TRealProvider).Name;
            var redirectUri = new Uri($"http://localhost:{port}/oauth/{providerName}");

            var oauth2 = new OAuth2App<TMockProvider>(
                clientId,
                redirectUri.ToString());
            scopes(oauth2);

            var mock = oauth2
                .GetPropertyValue<TMockProvider>("Provider");

            using (var server = new OAuthTestingServer<OAuth2Token>())
            {
                server
                    .AddApplicationId(clientId)
                    .AddHandler(
                        mock.AuthorizationUrl,
                        new OAuth2AuthorizationHandler(
                            clientId,
                            redirectUri,
                            new OAuth2TokenRedirectBuilder(),
                            new OAuth2TokenCredentialBuilder(
                                server.Tokens,
                                3600),
                            requiresScope));

                var serverTask = server.Start(mock.Port);

                var tokenTask = oauth2.GetCredentialsAsync();

                await Task.WhenAny(serverTask, tokenTask)
                    .ConfigureAwait(false);

                if (serverTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.Null(serverTask.Result);
                }

                if (tokenTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.True(ValidationUtilities.IsValidOAuth2Token(tokenTask.Result));
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
}
