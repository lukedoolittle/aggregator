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
    
    public class PollingIntervalChangedEventHandlerTests
    {
        [Fact]
        public void HandleSensorCreatedEventThrown()
        {
            var databaseMock = new DatabaseMock<SensorDto>();
            var sensorId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();
            var oldPollingInterval = PollingIntervalEnum.Moderate;
            var sensor = new SensorDto(
               sensorId,
               aggregateId,
               oldPollingInterval,
               new JObject(),
               typeof(Facebook),
               typeof(FacebookActivity),
               null,
               0);
            databaseMock.Put(sensor);

            var newPollingInterval = PollingIntervalEnum.Fast;
            var @event = new PollingIntervalChanged<FacebookActivity>(
                newPollingInterval);
            @event.AggregateId = aggregateId;

            var handler = new PollingIntervalChangedEventHandler<FacebookActivity>(databaseMock);

            handler.Handle(@event);

            var actualSensor = databaseMock.GetAll(aggregateId.ToString()).Single(s => s.Id == sensorId);
            Assert.Equal(newPollingInterval, actualSensor.PollingInterval);
        }
    }
}
