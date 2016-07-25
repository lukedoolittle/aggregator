﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Foundations.Bootstrap;
using SimpleCQRS.Framework.Contracts;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Infrastructure.Messaging
{
    using System.Threading.Tasks;

    public class DynamicSubscriptionMessageBusDecorator : MessageBus
    {
        private readonly IComponentContext _container;
        private readonly List<Type> _genericSubscriptionTypes;

        public DynamicSubscriptionMessageBusDecorator(IComponentContext container)
        {
            _container = container;
            _genericSubscriptionTypes = new List<Type>();
        }

        public override Task Publish<TEvent>(TEvent @event)
        {
            var newHandlerTypes = _container
                .ResolutionTypes<IEventHandler<TEvent>>()
                .Where(t => !_genericSubscriptionTypes.Contains(t))
                .ToList();

            var newHandlers = newHandlerTypes
                .Select(t => (IEventHandler<TEvent>)_container.Resolve(t));

            foreach (var handler in newHandlers)
            {
                //TODO: should we return subscription tokens here for new subscriptions??
                Subscribe<TEvent>(handler.Handle);
            }

            _genericSubscriptionTypes.AddRange(newHandlerTypes);

            return base.Publish<TEvent>(@event);
        }

        public override Task Send<TCommand>(TCommand command)
        {
            var newHandlerTypes = _container
                .ResolutionTypes<ICommandHandler<TCommand>>()
                .Where(t => !_genericSubscriptionTypes.Contains(t))
                .ToList();

            var newHandlers = newHandlerTypes
                .Select(t => (ICommandHandler<TCommand>)_container.Resolve(t));

            foreach (var handler in newHandlers)
            {
                //TODO: should we return subscription tokens here for new subscriptions??
                Subscribe<TCommand>(handler.Handle);
            }

            _genericSubscriptionTypes.AddRange(newHandlerTypes);

            return base.Send<TCommand>(command);
        }
    }
}
