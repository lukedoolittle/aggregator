using System;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aggregator.Configuration;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Task;
using Aggregator.Task.Commands;
using Aggregator.Test.Helpers;
using Foundations.Http;
using Material.Contracts;
using SimpleCQRS.Framework.Contracts;
using Xunit;

namespace Aggregator.Test.Integration.Configuration
{
    [CollectionDefinition("Database collection")]
    public class BootstraperTests : 
        IClassFixture<BootstrapFixture>
    {
        private readonly IServiceLocator _resolver;

        public BootstraperTests(BootstrapFixture fixture)
        {
            _resolver = fixture.Resolver;
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

        [Fact]
        public void ResolveServers()
        {
            Assert.NotNull(_resolver.GetInstance<HttpServer>());
        }

        [Fact]
        public void ResolveAuthorizers()
        {
            Assert.NotNull(_resolver.GetInstance<IOAuthAuthorizerUI>());
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
            Assert.NotNull(_resolver.GetInstance<IBluetoothAdapter>());
            Assert.NotNull(_resolver.GetInstance<IGPSAdapter>());
#if __ANDROID__
            Assert.NotNull(_resolver.GetInstance<ISMSAdapter>());
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
    }
}
