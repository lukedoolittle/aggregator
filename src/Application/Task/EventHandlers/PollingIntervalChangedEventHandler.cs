using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class PollingIntervalChangedEventHandler<TRequest> :
        IEventHandler<PollingIntervalChanged<TRequest>>
        where TRequest : Request, new()
    {
        private readonly IDatabase<SensorDto> _database;

        public PollingIntervalChangedEventHandler(IDatabase<SensorDto> database)
        {
            _database = database;
        }

        public void Handle(PollingIntervalChanged<TRequest> @event)
        {
            var sensor = _database
                .GetAll(@event.AggregateId.ToString())
                .Single(s => s.SensorType == typeof (TRequest));
            sensor.PollingInterval = @event.NewPollingInterval;
            sensor.Version = @event.Version;
            _database.Update(sensor);
        }
    }
}
