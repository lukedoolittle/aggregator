using System;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Requests;
using Aggregator.Test.Fixtures;
using Aggregator.Test.Helpers;
using Xunit;

namespace Aggregator.Test.Integration.Factories
{
    public class ClientFactoryTests : IClassFixture<ClientFactoryFixture>
    {
        private readonly IClientFactory _factory;

        public ClientFactoryTests(ClientFactoryFixture fixture)
        {
            _factory = fixture.Factory;
        }

#if __ANDROID__
        [Fact]
        public void CreateUnAuthenticatedSmsClient()
        {
            var client = _factory.CreateClient(typeof (SMSTextMessage));

            Assert.NotNull(client);
        }
#endif

#if __MOBILE__
        [Fact]
        public void CreateUnauthenticatedGpsClient()
        {
            var client = _factory.CreateClient(typeof(GPSPosition));

            Assert.NotNull(client);
        }

        [Fact]
        public void CreateAuthenticatedBluetoothClient()
        {
            var client = _factory.CreateClient(
                typeof(MioHeartRate), 
                new BluetoothCredentials());

            Assert.NotNull(client);
        }
#endif

        [Fact]
        public void CreateAuthenticatedOAuth2Client()
        {
            var credentials = new OAuth2Credentials();
            credentials.SetTokenName(Guid.NewGuid().ToString());
            credentials.SetValue("_accessToken", Guid.NewGuid().ToString());

            var client = _factory.CreateClient(
                typeof(FacebookFeed),
                credentials);

            Assert.NotNull(client);
        }

        [Fact]
        public void CreateAuthenticatedOAuth1Client()
        {
            var credentials = new OAuth1Credentials();
            credentials.SetConsumerProperties(
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString());
            credentials.SetValue("OAuthToken", Guid.NewGuid().ToString());
            credentials.SetValue("OAuthSecret", Guid.NewGuid().ToString());

            var client = _factory.CreateClient(
                typeof(TwitterTweet),
                credentials);

            Assert.NotNull(client);
        }
    }
}
