using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class FilterChangedEventHandler<TRequest> :
        IEventHandler<FilterChanged<TRequest>>
        where TRequest : Request, new()
    {
        private readonly IDatabase<SensorDto> _database;

        public FilterChangedEventHandler(IDatabase<SensorDto> database)
        {
            _database = database;
        }

        public void Handle(FilterChanged<TRequest> @event)
        {
            var sensor = _database
                .GetAll(@event.AggregateId.ToString())
                .Single(s => s.SensorType == typeof (TRequest));
            sensor.LastSample = @event.NewFilter;
            sensor.Version = @event.Version;
            _database.Update(sensor);
        }
    }
}
