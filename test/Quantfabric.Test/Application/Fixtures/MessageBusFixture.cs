using System;
using Aggregator.Infrastructure.Messaging;
using Autofac;
using Autofac.Features.ResolveAnything;
using SimpleCQRS.Infrastructure;
using Foundations;
using Foundations.Bootstrap;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Integration
{
    public class MessageBusFixture : IDisposable
    {
        private MessageBus _bus;
        public ContainerBuilder Builder;

        public MessageBus Bus
        {
            get
            {
                if (_bus == null)
                {
                    _bus = new DynamicSubscriptionMessageBusDecorator(Builder.Build());
                }

                return _bus;
            }
        }

        public MessageBusFixture()
        {
            var assemblies = CurrentAppDomain.GetAssemblies();

            Builder = new ContainerBuilder();

            Builder.RegisterAssemblyGenericInterfaceImplementors(
                assemblies,
                typeof(IEventHandler<>));
            Builder.RegisterAssemblyGenericInterfaceImplementors(
                assemblies,
                typeof(ICommandHandler<>));

            Builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
        }

        public void Dispose()
        {
        }
    }
}
