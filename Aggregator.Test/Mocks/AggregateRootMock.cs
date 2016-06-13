using System;
using SimpleCQRS.Domain;

namespace Aggregator.Test.Helpers.Mocks
{
    class AggregateRootMock : AggregateRoot
    {
        public override Guid Id { get; }

        public Event MyEvent { get; private set; }

        public AggregateRootMock()
        {
        }

        public AggregateRootMock(Guid id) : this()
        {
            Id = id;
        }

        protected override void RegisterConflictResolvers()
        {
            throw new NotImplementedException();
        }

        protected void OnSomeEvent(Event @event)
        {
            MyEvent = @event;
        }

        public void SomeEvent(Event @event)
        {
            ApplyChange(@event);
        }
    }
}
