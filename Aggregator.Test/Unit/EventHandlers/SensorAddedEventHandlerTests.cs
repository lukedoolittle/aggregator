using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Requests;
using Aggregator.Infrastructure.Services;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
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
            var pollingInterval = PollingIntervalEnum.Moderate;
            var authenticationToken = new JObject();
            var @event = new SensorCreated<FacebookActivity>(
                sensorId,
                pollingInterval,
                authenticationToken);
            @event.AggregateId = aggregateId;

            var handler = new SensorAddedEventHandler<FacebookActivity>(databaseMock);

            handler.Handle(@event);

            var sensors = databaseMock.GetAll(aggregateId.ToString());
            Assert.Equal(1, sensors.Count());
            var sensor = sensors.First();
            Assert.Equal(sensor.IsActive, true);
            Assert.Equal(sensor.SensorType.Name, typeof(FacebookActivity).Name);
        }
    }
}
