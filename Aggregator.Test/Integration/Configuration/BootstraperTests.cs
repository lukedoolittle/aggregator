using System;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aggregator.Configuration;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Requests;
using Aggregator.Infrastructure.Services;
using Aggregator.Task;
using Aggregator.Task.Commands;
#if !__MOBILE__
using Aggregator.Task.Http;
#endif
using Aggregator.Test.Helpers;
using SimpleCQRS.Framework.Contracts;
using Xunit;

namespace Aggregator.Test.Integration.Configuration
{
    [CollectionDefinition("Database collection")]
    public class BootstraperTests : 
        IClassFixture<BootstrapFixture>
    {
        //private readonly TrackingMessageBusDecorator _bus;
        private readonly IServiceLocator _resolver;

        public BootstraperTests(BootstrapFixture fixture)
        {
            _resolver = fixture.Resolver;
            //_bus = (TrackingMessageBusDecorator)_resolver.GetInstance<ICommandSender>();
        }

        [Fact]
        public void ResolveStartupTasks()
        {
            Assert.NotNull(_resolver.GetInstance<PopulateSchedulerStartupTask>());
        }

        [Fact]
        public void ResolveTaskScheduler()
        {
            Assert.NotNull(_resolver.GetInstance<Scheduler>());
        }

        [Fact]
        public void ResolveMessageBus()
        {
            var publisher = _resolver.GetInstance<IEventPublisher>();
            var sender = _resolver.GetInstance<ICommandSender>();
            var manager = _resolver.GetInstance<ISubscriptionManager>();

            Assert.NotNull(publisher);
            Assert.True(publisher == sender);
            Assert.True(sender == manager);
        }

        [Fact]
        public void ResolveEventStore()
        {
            Assert.NotNull(_resolver.GetInstance<IEventStore>());
        }

        [Fact]
        public void ResolveRepositories()
        {
            Assert.NotNull(_resolver.GetInstance<IRepository<Person>>());
            Assert.NotNull(_resolver.GetInstance<ReadModelFacade>());
        }

        [Fact]
        public void ResolveFactories()
        {
            Assert.NotNull(_resolver.GetInstance<IAuthenticationRefreshTaskFactory>());
            Assert.NotNull(_resolver.GetInstance<ISampleRequestTaskFactory>());
            Assert.NotNull(_resolver.GetInstance<IClientCredentials>());
            Assert.NotNull(_resolver.GetInstance<IClientFactory>());
            Assert.NotNull(_resolver.GetInstance<IOAuthFactory>());
        }

#if !__MOBILE__
        [Fact]
        public void ResolveServers()
        {
            Assert.NotNull(_resolver.GetInstance<HttpServer>());
        }
#endif

        [Fact]
        public void ResolveAuthorizers()
        {
            Assert.NotNull(_resolver.GetInstance<IWebAuthorizer>());
        }

        [Fact]
        public void ResolveLoggers()
        {
            Assert.NotNull(_resolver.GetInstance<ILogger>());
        }

#if __MOBILE__
        [Fact]
        public void ResolveManagers()
        {
            Assert.NotNull(_resolver.GetInstance<IBluetoothManager>());
            Assert.NotNull(_resolver.GetInstance<IGPSManager>());
#if __ANDROID__
            Assert.NotNull(_resolver.GetInstance<ISMSManager>());
#endif
        }
#endif
        [Fact]
        public void CheckSerializationInitialization()
        {
            var serializerSettings = JsonConvert.DefaultSettings();

            Assert.NotNull(serializerSettings);
        }

        [Fact]
        public void CheckUserInitialization()
        {
            var myId = UserSettings.UserId;

            Assert.NotEqual(Guid.Empty, myId);
        }

        [Fact]
        public void CheckPlatformInitialization()
        {
        }

        //[Fact]
        //public async void CommandHandlersAreProperlySubscribed()
        //{
        //    var publisher = _resolver.GetInstance<ICommandSender>();

        //    Assert.Equal(publisher, _bus);

        //    await publisher.Send(new CreatePersonCommand(Guid.NewGuid(), -1));
        //    await publisher.Send(new CreateTokenCommand<Facebook>(Guid.NewGuid(), new JObject(), -1));

        //    Assert.Equal(1, _bus.PublishCount(typeof(CreatePersonCommand)));
        //    Assert.Equal(1, _bus.PublishCount(typeof(CreateTokenCommand<Facebook>)));
        //}

        //[Fact]
        //public void EventHandlersAreProperlySubscribed()
        //{
        //    var publisher = _resolver.GetInstance<IEventPublisher>();

        //    Assert.Equal(publisher, _bus);

        //    publisher.Publish(new TokenCreated<Facebook>(new JObject()));
        //    publisher.Publish(new TokenUpdated<Facebook>(new JObject()));
        //    publisher.Publish(new SensorCreated<FacebookActivity>(Guid.NewGuid(), PollingIntervalEnum.Fast, new JObject()));

        //    Assert.Equal(1, _bus.PublishCount(typeof(TokenCreated<Facebook>)));
        //    Assert.Equal(1, _bus.SubscribeCount(typeof(TokenCreated<Facebook>)));

        //    Assert.Equal(1, _bus.PublishCount(typeof(TokenUpdated<Facebook>)));

        //    Assert.Equal(1, _bus.PublishCount(typeof(SensorCreated<FacebookActivity>)));
        //}
    }
}
