using System;
using Autofac;
using Autofac.Features.ResolveAnything;
using BatmansBelt;
using BatmansBelt.Extensions;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Requests;
using Aggregator.Infrastructure.Services;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using SimpleCQRS.Autofac;
using SimpleCQRS.Framework.Contracts;
using Xunit;

namespace Aggregator.Test.Integration
{
    
    public class ReflectionWithEventingTests
    {
        [Fact]
        public async void SubscribeToGenericEventAllowsEventToBeHandled()
        {
            var sensorId = Guid.NewGuid();
            var databaseMock = new DatabaseMock<SensorDto>();
            var token = new JObject();
            var pollingInterval = PollingIntervalEnum.Fast;
            databaseMock.Put(new SensorDto(
                sensorId, 
                Guid.Empty,
                pollingInterval,
                token, 
                typeof(Facebook), 
                typeof(FacebookActivity),
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

            var subscriptionManager = new DynamicSubscriptionMessageBus(builder.Build());
            

            var @event = new FilterChanged<FacebookActivity>("somefilter");

            await subscriptionManager.Publish(@event);

            var actualSensor = databaseMock.Get(sensorId.ToString());

            Assert.Equal(token, actualSensor.Token);
        }
    }
}

