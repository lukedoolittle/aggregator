using System;
using System.Collections.Generic;
using Autofac;
using SimpleCQRS.Autofac;
using SimpleCQRS.Framework;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test
{
    using System.Threading.Tasks;

    public class TrackingMessageBusDecorator : DynamicSubscriptionMessageBus
    {
        private readonly IDictionary<Type, MessageBusInvocations> _subscriptionCount;
        private readonly HashSet<string> _unboundTypeNames;
        private readonly bool _actuallyPublish;

        public TrackingMessageBusDecorator(IComponentContext context) : base(context)
        {
            _subscriptionCount = new Dictionary<Type, MessageBusInvocations>();
            _unboundTypeNames = new HashSet<string>();
            _actuallyPublish = false;
        }

        public int PublishCount(Type type)
        {
            if (_subscriptionCount.ContainsKey(type))
            {
                return _subscriptionCount[type].PublishCount;
            }
            else if (_subscriptionCount.ContainsKey(type.Unbind()))
            {
                return _subscriptionCount[type.Unbind()].PublishCount;
            }
            else
            {
                return 0;
            }
        }

        public int SubscribeCount(Type type)
        {
            if (_subscriptionCount.ContainsKey(type))
            {
                return _subscriptionCount[type].SubscriptionCount;
            }
            else if (_subscriptionCount.ContainsKey(type.Unbind()))
            {
                return _subscriptionCount[type.Unbind()].SubscriptionCount;
            }
            else
            {
                return 0;
            }
        }

        public override Task Publish<T>(T @event) 
        {
            if (_subscriptionCount.ContainsKey(typeof (T)))
            {
                _subscriptionCount[typeof (T)].PublishCount++;
            }
            else if (_subscriptionCount.ContainsKey(typeof(T).Unbind()))
            {
                _subscriptionCount[typeof(T).Unbind()].PublishCount++;
            }
            else
            {
                _subscriptionCount.Add(typeof(T), new MessageBusInvocations(0,1));
            }

            if (_actuallyPublish)
            {
                return base.Publish(@event);
            }

            return Task.FromResult(0);
        }

        public override Task Send<T>(T command) 
        {
            if (_subscriptionCount.ContainsKey(typeof(T)))
            {
                _subscriptionCount[typeof(T)].PublishCount++;
            }
            else if (_subscriptionCount.ContainsKey(typeof(T).Unbind()))
            {
                _subscriptionCount[typeof(T).Unbind()].PublishCount++;
            }

            if (_actuallyPublish)
            {
                return base.Send(command);
            }

            return Task.FromResult(0);
        }

        public override ISubscription Subscribe<TMessage>(Action<TMessage> action)
        {
            _unboundTypeNames.Add(typeof (TMessage).GetNonGenericName());

            if (_subscriptionCount.ContainsKey(typeof (TMessage)))
            {
                _subscriptionCount[typeof (TMessage)].SubscriptionCount++;
            }
            else
            {
                _subscriptionCount.Add(typeof(TMessage), new MessageBusInvocations(1,0));
            }

            return base.Subscribe(action);
        }

        public override void UnSubscribe(ISubscription subscription)
        {
            base.UnSubscribe(subscription);
        }
    }

    public class MessageBusInvocations
    {
        public int SubscriptionCount { get; set; }

        public int PublishCount { get; set; }

        public MessageBusInvocations(int subscriptionCount, int publishCount)
        {
            SubscriptionCount = subscriptionCount;
            PublishCount = publishCount;
        }
    }
}
