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
    
    public class SensorReactivatedEventHandlerTests
    {
        [Fact]
        public void HandleSensorReactivatedEventThrow()
        {
            var aggregateId = Guid.NewGuid();
            var sensorId = Guid.NewGuid();
            var databaseMock = new DatabaseMock<SensorDto>();
            var sensor = new SensorDto(
                sensorId,
                aggregateId,
                TimeSpan.Zero, 
                new JObject(),
                typeof(Facebook),
                typeof(FacebookEvent),
                null,
                0);
            sensor.IsActive = false;
            databaseMock.Put(sensor);

            var @event = new SensorReactivated<FacebookEvent>(sensorId) { AggregateId = aggregateId };

            var eventHandler = new SensorReactivatedEventHandler<FacebookEvent>(databaseMock);

            eventHandler.Handle(@event);

            var actualSensor = databaseMock.GetAll(aggregateId.ToString()).Single(s => s.Id == sensorId);
            Assert.True(actualSensor.IsActive);
        }
    }
}
