using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Requests;
using Aggregator.Infrastructure.Services;
using Aggregator.Task;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers;
using Aggregator.Test.Helpers.Mocks;
using Aggregator.Test.Mocks;
using Xunit;

namespace Aggregator.Test.Unit.EventHandlers
{
    public class LivePollingIntervalChangedEventHandlerTests
    {
        [Fact]
        public void HandleSensorCreatedEventThrown()
        {
            var aggregateId = Guid.NewGuid();
            var oldPolling = 10;
            var newPolling = 15;
            RequestMock.Interval = new PollingInterval(newPolling, oldPolling, 0);
            var oldPollingInterval = PollingIntervalEnum.Moderate;
            var authenticationToken = new JObject();
            var sensorId = Guid.NewGuid();
            var sensorType = typeof(RequestMock);
            var serviceType = typeof(ServiceMock);

            var databaseMock = new DatabaseMock<SensorDto>();
            var sensor = new SensorDto(
               sensorId,
               aggregateId,
               oldPollingInterval,
               authenticationToken,
               serviceType,
               sensorType,
               null,
               0);
            databaseMock.Put(sensor);

            var scheduler = new Scheduler();
            scheduler.AddTask(sensorId, new TaskMock(new EventPublisherMock()), oldPolling, true);

            var newPollingInterval = PollingIntervalEnum.Fast;
            var @event = new PollingIntervalChanged<RequestMock>(
                newPollingInterval);
            @event.AggregateId = aggregateId;

            var handler = new LivePollingIntervalChangedEventHandler<RequestMock>(
                scheduler, 
                databaseMock);

            handler.Handle(@event);

            var task = scheduler
                .GetMemberValue<List<RepeatingTask>>("_tasks")
                .SingleOrDefault(t => t.TaskId == sensorId);
                        Assert.NotNull(task);
            var actualRepetitionPeriod = task.GetMemberValue<TimeSpan>("_repetitionPeriod");
            Assert.Equal(
                TimeSpan.FromMilliseconds(newPolling),
                actualRepetitionPeriod);
        }
    }
}
