using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class SensorReactivatedEventHandler<TRequest> : 
        IEventHandler<SensorReactivated<TRequest>>
        where TRequest : Request, new()
    {
        private readonly IDatabase<SensorDto> _database;

        public SensorReactivatedEventHandler(IDatabase<SensorDto> database)
        {
            _database = database;
        }

        public void Handle(SensorReactivated<TRequest> @event)
        {
            var sensor = _database
                .GetAll(@event.AggregateId.ToString())
                .Single(s => s.Id == @event.SensorId);
            sensor.IsActive = true;
            sensor.Version = @event.Version;
            _database.Update(sensor);
        }
    }
}
