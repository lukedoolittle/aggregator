using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class TokenUpdatedEventHandler<TService> : IEventHandler<TokenUpdated<TService>>
        where TService : ResourceProvider
    {
        private readonly IDatabase<SensorDto> _database;

        public TokenUpdatedEventHandler(IDatabase<SensorDto> database)
        {
            _database = database;
        }

        public void Handle(TokenUpdated<TService> @event)
        {
            var sensors = _database
                .GetAll(@event.AggregateId.ToString())
                .Where(s => s.ServiceType.Name == @event.ServiceName);

            foreach (var sensor in sensors)
            {
                sensor.Token = @event.Values;
                sensor.Version = @event.Version;
                _database.Update(sensor);
            }
        }
    }
}
