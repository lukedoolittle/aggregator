using System;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Factories;
using Aggregator.Test.Helpers;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Application.Mocks;
using Xunit;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;

namespace Aggregator.Test.Integration.Factories
{
    public class ClientFactoryTests
    {
#if __ANDROID__
        [Fact]
        public void CreateUnAuthenticatedSmsClient()
        {
            var client = CreateClientFactory().CreateClient<SMSRequest>();

            Assert.NotNull(client);
        }
#endif

#if __MOBILE__
        [Fact]
        public void CreateUnauthenticatedGpsClient()
        {
            var client = CreateClientFactory().CreateClient<GPSRequest>();

            Assert.NotNull(client);
        }

        [Fact]
        public void CreateAuthenticatedBluetoothClient()
        {
            var client = CreateClientFactory()
                    .CreateClient<MioHeartRate, BluetoothCredentials>(
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

            var client = CreateClientFactory()
                .CreateClient<FacebookFeed, OAuth2Credentials>(
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

            var client = CreateClientFactory()
                .CreateClient<TwitterTweet, OAuth1Credentials>(
                    credentials);

            Assert.NotNull(client);
        }

        private ClientFactory CreateClientFactory()
        {
            var bluetoothManagerMock = new BluetoothAdapterMock();
            var gpsMock = new IGPSAdapterMock();
            var oauthMock = new OAuthFactoryMock();
            var smsMock = new SMSManagerMock();

            return new ClientFactory(
                smsMock, 
                bluetoothManagerMock, 
                gpsMock, 
                oauthMock);
        }
    }
}
