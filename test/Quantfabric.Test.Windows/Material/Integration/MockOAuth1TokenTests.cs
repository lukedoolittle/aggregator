﻿using System;
using System.Threading.Tasks;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Quantfabric.Test.Material.Mocks;
using Quantfabric.Test.Material.OAuthServer;
using Quantfabric.Test.Material.OAuthServer.Builders;
using Quantfabric.Test.Material.OAuthServer.Handlers;
using Quantfabric.Test.Material.OAuthServer.Serialization;
using Quantfabric.Test.Material.OAuthServer.Tokens;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    public class MockOAuth1TokenTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void CanGetValidAccessTokenFromTwitter()
        {
            await RunServer<Twitter, TwitterMock>(true)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFatsecret()
        {
            await RunServer<Fatsecret, FatsecretMock>(false)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTumblr()
        {
            await RunServer<Tumblr, TumblrMock>(false)
                .ConfigureAwait(false);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromWithings()
        {
            await RunServer<Withings, WithingsMock>(true)
                .ConfigureAwait(false);
        }

        private async Task RunServer<TRealProvider, TMockProvider>(
            bool shouldContainUserId)
            where TRealProvider : OAuth1ResourceProvider
            where TMockProvider : OAuth1ResourceProviderMock, new()
        {
            var consumerKey = _appRepository.GetConsumerKey<TRealProvider>();
            var consumerSecret = _appRepository.GetConsumerSecret<TRealProvider>();

            var port = TestUtilities.GetAvailablePort(35000);
            var providerName = typeof (TRealProvider).Name;
            var redirectUri = new Uri($"http://localhost:{port}/oauth/{providerName}");

            var oauth1 = new OAuth1App<TMockProvider>(
                consumerKey,
                consumerSecret,
                redirectUri.ToString());

            var mock = oauth1
                .GetMemberValue<OAuth1AppBase<TMockProvider>>("_app")
                .GetMemberValue<TMockProvider>("_provider");

            IIncommingMessageDeserializer deserializer;
            if (mock.ParameterType == HttpParameterType.Querystring)
            {
                deserializer = new QuerystringMessageDeserializer();
            }
            else
            {
                deserializer = new HtmlBodyMessageDeserializer();
            }

            using (var server = new OAuthTestingServer<OAuth1Token>())
            {
                server
                    .AddApplicationId(consumerKey)
                    .AddHandler(
                        mock.RequestUrl,
                        new OAuth1RequestTokenHandler(
                            consumerKey,
                            redirectUri,
                            new OAuth1SignatureVerifier(consumerSecret), 
                            deserializer,
                            new OAuth1RequestTokenCredentialBuilder(
                                server.Tokens)))
                    .AddHandler(
                        mock.AuthorizationUrl,
                        new OAuth1AuthorizationHandler(
                            redirectUri,
                            new OAuth1AuthorizationRedirectBuilder(), 
                            new OAuth1AuthenticationCredentialBuilder(
                                server.Tokens)))
                    .AddHandler(
                        mock.TokenUrl,
                        new OAuth1AccessTokenHandler(
                            consumerKey,
                            new OAuth1SignatureVerifier(consumerSecret),
                            server.Tokens,
                            deserializer,
                            new OAuth1AccessTokenCredentialBuilder(
                                server.Tokens,
                                shouldContainUserId)));

                var serverTask = server.Start(mock.Port);

                var tokenTask = oauth1.GetCredentialsAsync();

                await Task.WhenAny(serverTask, tokenTask)
                    .ConfigureAwait(false);

                if (serverTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.Null(serverTask.Result);
                }

                if (tokenTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.True(IsValidToken(
                        tokenTask.Result, 
                        shouldContainUserId));
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private static bool IsValidToken(
            OAuth1Credentials token,
            bool shouldContainUserId)
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
    }
}