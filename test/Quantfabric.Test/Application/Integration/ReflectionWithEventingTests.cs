using System;
using Autofac;
using Autofac.Features.ResolveAnything;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Infrastructure.Messaging;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using Foundations;
using Foundations.Bootstrap;
using SimpleCQRS.Autofac;
using SimpleCQRS.Framework.Contracts;
using Xunit;
using Aggregator.Test.Integration;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;

namespace Aggregator.Test.Integration
{

    public class ReflectionWithEventingTests : IClassFixture<MessageBusFixture>
    {
        private readonly MessageBusFixture _fixture;

        public ReflectionWithEventingTests(MessageBusFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void SubscribeToGenericEventAllowsEventToBeHandled()
        {
            var sensorId = Guid.NewGuid();
            var databaseMock = new DatabaseMock<SensorDto>();
            var token = new JObject();
            var pollingInterval = TimeSpan.Zero;
            databaseMock.Put(new SensorDto(
                sensorId, 
                Guid.Empty,
                pollingInterval,
                token, 
                typeof(Facebook), 
                typeof(FacebookEvent),
                "", 
                -1));
            var assemblies = CurrentAppDomain.GetAssemblies();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyGenericInterfaceImplementors(
                assemblies,
                typeof(IEventHandler<>));
            builder.RegisterAssemblyGenericInterfaceImplementors(
                assemblies,
                typeof(ICommandHandler<>));
            builder.RegisterInstance(databaseMock).As<IDatabase<SensorDto>>();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var subscriptionManager = new DynamicSubscriptionMessageBusDecorator(builder.Build());
            

            var @event = new FilterChanged<FacebookEvent>("somefilter");

            await subscriptionManager.Publish(@event);

            var actualSensor = databaseMock.Get(sensorId.ToString());

            Assert.Equal(token, actualSensor.Token);
        }
    }
}

