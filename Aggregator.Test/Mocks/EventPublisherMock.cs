﻿using LightMock;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Helpers.Mocks
{
    using System.Threading.Tasks;

    public class EventPublisherMock : MockBase<IEventPublisher>, IEventPublisher
    {
        private object _lastPublishedObject;

        public Task Publish<T>(T @event) 
            where T : Event
        {
            _invoker.Invoke(a => a.Publish(@event));
            _lastPublishedObject = @event;
            return Task.FromResult(0);
        }

        public void AssertPublishCountAtLeast<T>(int count)
             where T : Event
        {
            _context.Assert(
                a => a.Publish(The<T>.IsAnyValue),
                Invoked.AtLeast(count));
        }

        public void AssertPublishCount<T>(int count)
            where T : Event
        {
            _context.Assert(
                a => a.Publish(The<T>.IsAnyValue),
                Invoked.Exactly(count));
        }

        public T GetLastPublishedObject<T>()
        {
            if (_lastPublishedObject == null)
            {
                return default(T);
            }
            return (T)_lastPublishedObject;
        }
    }
}
