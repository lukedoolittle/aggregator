using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class LivePollingIntervalChangedEventHandler<TRequest> :
        IEventHandler<PollingIntervalChanged<TRequest>>
        where TRequest : Request, new()
    {
        private readonly Scheduler _scheduler;
        private readonly IDatabase<SensorDto> _database;

        public LivePollingIntervalChangedEventHandler(
            Scheduler scheduler,
            IDatabase<SensorDto> database)
        {
            _scheduler = scheduler;
            _database = database;
        }

        public void Handle(PollingIntervalChanged<TRequest> @event)
        {
            var sensor = _database
                .GetAll(@event.AggregateId.ToString())
                .Single(s => s.SensorType == typeof (TRequest));
            _scheduler.UpdatePollingInterval(
                sensor.Id,
                new TRequest().PollingInterval[@event.NewPollingInterval]);
        }
    }
}
