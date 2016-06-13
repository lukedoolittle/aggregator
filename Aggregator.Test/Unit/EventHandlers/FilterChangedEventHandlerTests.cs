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
    public class FilterChangedEventHandlerTests
    {
        [Fact]
        public void HandleSensorCreatedEventThrown()
        {
            var databaseMock = new DatabaseMock<SensorDto>();
            var sensorId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();
            var pollingInterval = PollingIntervalEnum.Moderate;
            var sensor = new SensorDto(
               sensorId,
               aggregateId,
               pollingInterval,
               new JObject(),
               typeof(Facebook),
               typeof(FacebookActivity),
               null,
               0);
            databaseMock.Put(sensor);

            var newFilter = "somenewfilter";
            var @event = new FilterChanged<FacebookActivity>(
                newFilter);
            @event.AggregateId = aggregateId;

            var handler = new FilterChangedEventHandler<FacebookActivity>(databaseMock);

            handler.Handle(@event);

            var actualSensor = databaseMock.GetAll(aggregateId.ToString()).Single(s => s.Id == sensorId);
            Assert.Equal(newFilter, actualSensor.LastSample);
        }
    }
}
