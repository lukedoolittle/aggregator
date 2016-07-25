using System.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Foundations.Extensions;
using Material.Infrastructure;
using Material.Metadata;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class SensorAddedEventHandler<TRequest> :
        IEventHandler<SensorCreated<TRequest>>
        where TRequest : Request, new()
    {
        private readonly IDatabase<SensorDto> _database;

        public SensorAddedEventHandler(IDatabase<SensorDto> database)
        {
            _database = database;
        }

        public void Handle(SensorCreated<TRequest> @event)
        {
            var request = typeof (TRequest);

            _database.Put(new SensorDto(
                @event.SensorId,
                @event.AggregateId,
                @event.PollingInterval,
                @event.AuthenticationToken,
                request.GetCustomAttributes<ServiceType>().Single().Type,
                request,
                null,
                @event.Version));
        }
    }
}
