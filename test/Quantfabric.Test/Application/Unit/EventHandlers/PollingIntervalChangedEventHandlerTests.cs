using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Xunit;

namespace Aggregator.Test.Unit.EventHandlers
{
    
    public class PollingIntervalChangedEventHandlerTests
    {
        [Fact]
        public void HandleSensorCreatedEventThrown()
        {
            var databaseMock = new DatabaseMock<SensorDto>();
            var sensorId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();
            var oldPollingInterval = TimeSpan.Zero;
            var sensor = new SensorDto(
               sensorId,
               aggregateId,
               oldPollingInterval,
               new JObject(),
               typeof(Facebook),
               typeof(FacebookEvent),
               null,
               0);
            databaseMock.Put(sensor);

            var newPollingInterval = TimeSpan.MaxValue;
            var @event = new PollingIntervalChanged<FacebookEvent>(
                newPollingInterval);
            @event.AggregateId = aggregateId;

            var handler = new PollingIntervalChangedEventHandler<FacebookEvent>(databaseMock);

            handler.Handle(@event);

            var actualSensor = databaseMock.GetAll(aggregateId.ToString()).Single(s => s.Id == sensorId);
            Assert.Equal(newPollingInterval, actualSensor.PollingInterval);
        }
    }
}
