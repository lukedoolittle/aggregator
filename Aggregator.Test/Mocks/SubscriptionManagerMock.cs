using System;
using System.Collections.Generic;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Helpers.Mocks
{
    public class SubscriptionManagerMock : MockBase<ISubscriptionManager>, ISubscriptionManager
    {
        public Dictionary<Type, object> RegisteredHandlers { get; }

        public SubscriptionManagerMock()
        {
            RegisteredHandlers = new Dictionary<Type, object>();
        }

        public ISubscription Subscribe<TMessage>(Action<TMessage> action) 
            where TMessage : IMessage
        {
            RegisteredHandlers.Add(typeof(TMessage), action);

            return _invoker.Invoke(a => a.Subscribe(action));
        }

        public ISubscription Subscribe(object handler, Type eventType, Type eventTypeGenericParameters = null)
        {
            throw new NotImplementedException();
        }

        public ISubscription OpenSubscribe(Type openGenericMessageType, Type openGenericEventHandlerType)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe(ISubscription subscription)
        {
            throw new NotImplementedException();
        }
    }
}
