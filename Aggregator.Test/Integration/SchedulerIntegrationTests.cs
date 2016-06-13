using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Requests;
using Aggregator.Infrastructure.Services;
using Aggregator.Task;
using Aggregator.Task.Requests;
using Aggregator.Test.Helpers;
using Aggregator.Test.Helpers.Mocks;
using Aggregator.Test.Mocks;
using Xunit;

namespace Aggregator.Test.Integration
{
    public class SchedulerIntegrationTests
    {
        [Fact]
        public void AddATaskToTheQueueAndStartTheQueueRunning()
        {
            var queryOne =
                new AuthenticatedClientMock<OAuth2Credentials>()
                .SetReturnValue(new List<Tuple<DateTimeOffset, JObject>>
                {
                    new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, new JObject())
                });
            var queryTwo = 
                new AuthenticatedClientMock<OAuth2Credentials>()
                .SetReturnValue(new List<Tuple<DateTimeOffset, JObject>>
                {
                    new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, new JObject())
                });
            var aggregateId = Guid.NewGuid();

            var credentials = new OAuth2Credentials();
            credentials.SetValue("_expiresIn", "10000");
            credentials.TimestampToken();

            var clientFactoryMockOne = new ClientFactoryMock()
                .SetReturnClient<FacebookActivity>(queryOne, credentials);
            var clientFactoryMockTwo = new ClientFactoryMock()
                .SetReturnClient<FacebookEvent>(queryTwo, credentials);

            var commandSenderMock = new CommandSenderMock();

            var eventPublisherMockOne = new EventPublisherMock();
            var eventPublisherMockTwo = new EventPublisherMock();

            var queue = new Scheduler();
            
            var taskMock = new TaskMock();

            var taskOne = new SampleRequestTask<FacebookActivity, OAuth2Credentials, Facebook>(
                clientFactoryMockOne,
                commandSenderMock, 
                eventPublisherMockOne,
                credentials, 
                taskMock,
                aggregateId, 
                string.Empty);
            var taskTwo = new SampleRequestTask<FacebookEvent, OAuth2Credentials, Facebook>(
                clientFactoryMockTwo,
                commandSenderMock,
                eventPublisherMockTwo,
                credentials,
                taskMock,
                aggregateId,
                string.Empty);
            queue.AddTask(aggregateId, taskOne, 100, true);
            queue.AddTask(aggregateId, taskTwo, 100, true);

            queue.Start();

            Thread.Sleep(1000);

            queue.Stop();

            queryOne.AssertMinimumNumberOfInvocations(1);
            queryTwo.AssertMinimumNumberOfInvocations(1);

            eventPublisherMockOne.AssertPublishCountAtLeast<SampleAdded<FacebookActivity>>(1);
            eventPublisherMockTwo.AssertPublishCountAtLeast<SampleAdded<FacebookEvent>>(1);
        }
    }
}
