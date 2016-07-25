using System;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Factories;
using Aggregator.Task.Requests;
using Aggregator.Test.Helpers;
using Foundations.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Xunit;
using Assert = Xunit.Assert;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;

namespace Aggregator.Test.Integration.Factories
{
    public class SampleRequestFactoryTests : IClassFixture<BootstrapFixture>
    {
        private readonly IServiceLocator _resolver;

        public SampleRequestFactoryTests(BootstrapFixture fixture)
        {
            _resolver = fixture.Resolver;
            _resolver.GetInstance<IOAuthFactory>();
        }

#if __ANDROID__
        [Fact]
        public void GenerateAOnboardTask()
        {
            var factory = ServiceLocator.Current.GetInstance<SampleRequestTaskFactory>();

            var task = factory.GetTask(
                Guid.NewGuid(),
                null,
                typeof(SMSRequest),
                string.Empty);

            Assert.NotNull(task);
            Assert.True(task is OnboardRequestTask<SMSRequest>);
        }
#endif
        [Fact]
        public void GenerateATask()
        {
            var factory = _resolver.GetInstance<ISampleRequestTaskFactory>();
            var aggregateId = Guid.NewGuid();

            var task = factory.GetTask(
                aggregateId,
                new OAuth2Credentials().AsJson().AsEntity<JObject>(), 
                typeof (FitbitSleep),
                string.Empty);

            Assert.NotNull(task);
            Assert.True(task is SampleRequestTask<FitbitSleep, OAuth2Credentials, Fitbit>);
        }

        [Fact]
        public void GenerateANonOAuth2Task()
        {
            var factory = _resolver.GetInstance<ISampleRequestTaskFactory>();

            var task = factory.GetTask(
                Guid.NewGuid(),
                new OAuth1Credentials().AsJson().AsEntity<JObject>(),
                typeof(TwitterTweet),
                string.Empty);

            Assert.NotNull(task);
            Assert.True(task is SampleRequestTask<TwitterTweet, OAuth1Credentials, Twitter>);
        }

#if __MOBILE__
        [Fact]
        public void GenerateABluetoothTask()
        {
            var factory = _resolver.GetInstance<ISampleRequestTaskFactory>();

            var task = factory.GetTask(
                Guid.NewGuid(),
                new JObject
                {
                    ["deviceAddress"] = Guid.NewGuid().ToString()
                },
                typeof(MioHeartRate),
                string.Empty);

            Assert.NotNull(task);
            Assert.True(task is SampleRequestTask<MioHeartRate, BluetoothCredentials, Mioalpha>);
        }
#endif
    }
}
