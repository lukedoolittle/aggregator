using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    public class MockOAuth2ImplicitTokenTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackTypeEnum.Localhost);

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            await RunServer<Google, GoogleMock>(
                    app => app.AddScope<GoogleGmail>()
                        .AddScope<GoogleGmailMetadata>()
                        .AddScope<GoogleProfile>()
                        .AddScope<GoogleAnalyticsReports>(),
                    server => server.SetRequiresScope())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebook()
        {
            await RunServer<Facebook, FacebookMock>(
                    app => app
                            .AddScope<FacebookEvent>()
                            .AddScope<FacebookFriend>(),
                    server => server.SetRequiresScope())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquare()
        {
            await RunServer<Foursquare, FoursquareMock>(
                app => { },
                server => { })
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotify()
        {
            await RunServer<Spotify, SpotifyMock>(
                    app => app
                            .AddScope<SpotifySavedTrack>(),
                    server => server.SetRequiresScope())
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromInstagram()
        {
            await RunServer<Instagram, InstagramMock>(
                    app => app
                            .AddScope<InstagramLikes>(),
                    server => server.SetRequiresScope())
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
                    server => server.SetRequiresScope())
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
            Action<OAuth2TestingServer> assertions)
            where TRealProvider : OAuth2ResourceProvider
            where TMockProvider : OAuth2ResourceProviderMock, new()
        {
            var clientId = _appRepository.GetClientId<TRealProvider>();

            var port = TestUtilities.GetAvailablePort(35000);
            var providerName = typeof(TRealProvider).Name;
            var redirectUri = $"http://localhost:{port}/oauth/{providerName}";

            var oauth2 = new OAuth2App<TMockProvider>(
                clientId,
                redirectUri);
            scopes(oauth2);

            var mock = oauth2.GetMemberValue<TMockProvider>("_provider");

            using (var server = new OAuth2TestingServer())
            {
                server
                    .SetClientId(clientId)
                    .SetCredentialsExpiration(3600)
                    .SetRedirectUri(redirectUri)
                    .SetAuthorizationPath(mock.AuthorizationUrl.AbsolutePath);
                assertions(server);

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
                    Assert.True(IsValidToken(tokenTask.Result));
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private bool IsValidToken(OAuth2Credentials token)
        {
            return token != null &&
                   token.AccessToken != string.Empty &&
                   token.TokenName != string.Empty;
        }
    }
}
