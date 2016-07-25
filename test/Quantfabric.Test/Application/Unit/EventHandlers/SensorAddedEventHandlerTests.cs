using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using Material.Infrastructure.Requests;
using Xunit;

namespace Aggregator.Test.Unit.EventHandlers
{
    
    public class SensorAddedEventHandlerTests
    {
        [Fact]
        public void HandleSensorCreatedEventThrown()
        {
            var databaseMock = new DatabaseMock<SensorDto>();
            var sensorId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();
            var pollingInterval = TimeSpan.Zero;
            var authenticationToken = new JObject();
            var @event = new SensorCreated<FacebookEvent>(
                sensorId,
                pollingInterval,
                authenticationToken);
            @event.AggregateId = aggregateId;

            var handler = new SensorAddedEventHandler<FacebookEvent>(databaseMock);

            handler.Handle(@event);

            var sensors = databaseMock.GetAll(aggregateId.ToString());
            Assert.Equal(1, sensors.Count());
            var sensor = sensors.First();
            Assert.Equal(sensor.IsActive, true);
            Assert.Equal(sensor.SensorType.Name, typeof(FacebookEvent).Name);
        }
    }
}
