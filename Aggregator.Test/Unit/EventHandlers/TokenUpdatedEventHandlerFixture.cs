using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Requests;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using Xunit;

namespace Aggregator.Test.Unit.EventHandlers
{
    using Aggregator.Infrastructure.Services;

    class TokenUpdatedEventHandlerFixture
    {
        [Fact]
        public void HandleTokenUpdatedEventThrow()
        {
            var aggregateId = Guid.NewGuid();
            var oldToken = new JObject();

            var sensorOne = new SensorDto(
                Guid.NewGuid(), 
                aggregateId, 
                PollingIntervalEnum.Moderate, 
                oldToken, 
                typeof(Facebook), 
                typeof(FacebookActivity), 
                null,
                0);
            var sensorTwo = new SensorDto(
                Guid.NewGuid(),
                aggregateId,
                PollingIntervalEnum.Moderate,
                oldToken,
                typeof(Facebook),
                typeof(FacebookActivity),
                null,
                0);
            var sensorThree = new SensorDto(
                Guid.NewGuid(),
                aggregateId,
                PollingIntervalEnum.Moderate,
                oldToken,
                typeof(Twitter),
                typeof(TwitterTweet),
                null,
                0);

            var databaseMock = new DatabaseMock<SensorDto>();
            databaseMock.Put(sensorOne);
            databaseMock.Put(sensorTwo);
            databaseMock.Put(sensorThree);

            var expected = new JObject();
            var @event = new TokenUpdated<Facebook>(expected) {AggregateId = aggregateId};

            var eventHandler = new TokenUpdatedEventHandler<Facebook>(databaseMock);

            eventHandler.Handle(@event);

            Assert.Equal(expected, sensorOne.Token);
            Assert.Equal(expected, sensorTwo.Token);
            Assert.Equal(oldToken, sensorThree.Token);
        }
    }
}
