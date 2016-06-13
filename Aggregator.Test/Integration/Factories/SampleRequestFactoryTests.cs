using System;
using BatmansBelt.Serialization;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Serialization;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Requests;
using Aggregator.Task.Factories;
using Aggregator.Task.Requests;
using Aggregator.Test.Helpers;
using Xunit;
using Assert = Xunit.Assert;

namespace Aggregator.Test.Integration.Factories
{
    using Aggregator.Infrastructure.Services;

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
                typeof(SMSTextMessage),
                string.Empty);

            Assert.NotNull(task);
            Assert.True(task is OnboardRequestTask<SMSTextMessage>);
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
