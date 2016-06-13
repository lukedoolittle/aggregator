using System;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class PersonCreated : Event
    {
        public Guid PersonId { get; private set; }

        public PersonCreated(Guid id)
        {
            PersonId = id;
            AggregateId = id;
        }
    }
}
