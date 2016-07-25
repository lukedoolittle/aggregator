using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Domain.Write.Samples;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class SampleAddedEventHandler<TRequest> :
        IEventHandler<SampleAdded<TRequest>>
        where TRequest : Request, new()
    {
        private readonly IDatabase<SampleDto<TRequest>> _database;

        public SampleAddedEventHandler(IDatabase<SampleDto<TRequest>> database)
        {
            _database = database;
        }

        public void Handle(SampleAdded<TRequest> @event)
        {
            //Using the payload hash as the ID and using Update instead of Put
            //allows us to avoid duplicate storage of samples in the database
            var sample = new SampleDto<TRequest>
            {
                Id = @event.PayloadHash,
                AggregateId = @event.AggregateId,
                Payload = @event.Payload,
                Timestamp = @event.Timestamp
            };
            _database.Update(sample);
        }
    }
}
