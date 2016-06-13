using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class SensorDeactivatedEventHandler<TRequest> : 
        IEventHandler<SensorDeactivated<TRequest>>
        where TRequest : Request, new()
    {
        private readonly IDatabase<SensorDto> _database;

        public SensorDeactivatedEventHandler(IDatabase<SensorDto> database)
        {
            _database = database;
        }

        public void Handle(SensorDeactivated<TRequest> @event)
        {
            var sensor = _database
                .GetAll(@event.AggregateId.ToString())
                .Single(s => s.Id == @event.SensorId);
            sensor.IsActive = false;
            sensor.Version = @event.Version;
            _database.Update(sensor);
        }
    }
}
