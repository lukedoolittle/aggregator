using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Framework.Exceptions;
using Aggregator.Task.Requests;
using Aggregator.Test.Helpers;
using Aggregator.Test.Helpers.Mocks;
using Aggregator.Test.Mocks;
using Foundations.Serialization;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using Xunit;

namespace Aggregator.Test.Unit.Tasks
{
    using System.Threading.Tasks;

    public class SampleRequestTaskTests
    {
        [Fact]
        public async void ExecutingAnExternalQueryTaskCallsQueryAndPublishsResult()
        {
            var expected = new JObject
            {
                ["somekey"] = "somevalue"
            };
            var credentialMock = new CredentialMock();

            var requestMock =
                new AuthenticatedClientMock<CredentialMock>()
                .SetReturnValue(
                    new List<Tuple<DateTimeOffset, JObject>>
                    {
                        new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, expected)
                    });
            var clientFactoryMock = new ClientFactoryMock().SetReturnClient<RequestMock, CredentialMock>(requestMock, credentialMock);

            var aggregateId = Guid.NewGuid();
            var commandSenderMock = new CommandSenderMock();
            var eventPublisherMock = new EventPublisherMock();

            var taskMock = new TaskMock();

            var task = new SampleRequestTask<RequestMock, CredentialMock, ResourceProviderMock>(
                clientFactoryMock,
                commandSenderMock,
                eventPublisherMock,
                credentialMock,
                taskMock,
                aggregateId,
                string.Empty);

            await task.Execute();

            requestMock.AssertMinimumNumberOfInvocations(1);
            eventPublisherMock.AssertPublishCount<SampleAdded<RequestMock>>(1);
            var actual = eventPublisherMock.GetLastPublishedObject<SampleAdded<RequestMock>>();
            Assert.NotNull(actual);
            var result = actual.Payload;
            Assert.NotNull(result);
            Assert.True(JToken.DeepEquals(expected, result));
        }

        [Fact]
        public async System.Threading.Tasks.Task ExecutingAnExternalQueryTaskWithExceptionThrowingRequestCapturesException()
        {
            var credentialMock = new CredentialMock();
            var requestMock =
                new AuthenticatedClientMock<CredentialMock>()
                .SetException<HttpRequestException>();
            var clientFactoryMock = new ClientFactoryMock().SetReturnClient<RequestMock, CredentialMock>(requestMock, credentialMock);

            var aggregateId = Guid.NewGuid();

            var commandSenderMock = new CommandSenderMock();
            var eventPublisherMock = new EventPublisherMock();

            var taskMock = new TaskMock();

            var task = new SampleRequestTask<RequestMock, CredentialMock, ResourceProviderMock>(
                clientFactoryMock,
                commandSenderMock,
                eventPublisherMock,
                credentialMock,
                taskMock,
                aggregateId,
                string.Empty);

            await Assert.ThrowsAsync<InvalidServiceRequestException<ResourceProviderMock>>(() => task.Execute());
        }

        [Fact]
        public async void ExecutingQueryTaskWithNoRecencyInRequestDoesNotUpdateLastQueryValue()
        {
            var expected = new JObject();
            var expectedLastQuery = DateTime.Now;
            expected["someRecency"] = expectedLastQuery;
            var credentialMock = new CredentialMock();
            var requestMock =
                new AuthenticatedClientMock<CredentialMock>()
                .SetReturnValue(
                    new List<Tuple<DateTimeOffset, JObject>>
                    {
                        new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, expected)
                    });
            var clientFactoryMock = new ClientFactoryMock().SetReturnClient<RequestMock, CredentialMock>(requestMock, credentialMock);

            var aggregateId = Guid.NewGuid();

            var commandSenderMock = new CommandSenderMock();
            var eventPublisherMock = new EventPublisherMock();

            var taskMock = new TaskMock();

            var task = new SampleRequestTask<RequestMock, CredentialMock, ResourceProviderMock>(
                clientFactoryMock,
                commandSenderMock,
                eventPublisherMock,
                credentialMock,
                taskMock,
                aggregateId,
                string.Empty);

            await task.Execute();

            var actualLastQuery = task.GetMemberValue<string>("_lastQuery");
            Assert.Equal("", actualLastQuery);
        }

        [Fact]
        public async void ExecutingQueryTaskWithNoRecencyValueInResultDoesNotUpdateLastQueryValue()
        {
            var requestMock =
                new AuthenticatedClientMock<CredentialMock>()
                .SetReturnValue(
                    new List<Tuple<DateTimeOffset, JObject>>
                    {
                        new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, new JObject())
                    });
            var credentialMock = new CredentialMock();
            var clientFactoryMock = new ClientFactoryMock().SetReturnClient<RequestMock, CredentialMock>(requestMock, credentialMock);

            var aggregateId = Guid.NewGuid();

            var commandSenderMock = new CommandSenderMock();
            var eventPublisherMock = new EventPublisherMock();

            var taskMock = new TaskMock();

            var task = new SampleRequestTask<RequestMock, CredentialMock, ResourceProviderMock> (
                clientFactoryMock,
                commandSenderMock,
                eventPublisherMock,
                credentialMock, 
                taskMock,
                aggregateId,
                string.Empty);

            await task.Execute();

            var actualLastQuery = task.GetMemberValue<string>("_lastQuery");
            Assert.Equal("", actualLastQuery);
        }

        [Fact]
        public async void ExecutingSampleRequestTaskWithExpiredCredentialsFetchesNewCredentialsBeforeMakingQuery()
        {
            var oldCredentials = new CredentialMock();
            oldCredentials.Id = Guid.NewGuid();
            oldCredentials.Expire();
            var newCredentials = new CredentialMock();
            newCredentials.Id = Guid.NewGuid();

            var clientMock =
                new AuthenticatedClientMock<CredentialMock>()
                .SetReturnValue(
                    new List<Tuple<DateTimeOffset, JObject>>
                    {
                        new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, new JObject())
                    });
            var clientFactoryMock = new ClientFactoryMock().SetReturnClientDontCareAboutCredentials<RequestMock, CredentialMock>(clientMock);

            var aggregateId = Guid.NewGuid();

            var commandSenderMock = new CommandSenderMock();
            var eventPublisherMock = new EventPublisherMock();

            var taskMock = new TaskMock();
            var @event = new TokenUpdated<ResourceProviderMock>(
                newCredentials.AsJObject(),
                Guid.NewGuid(),
                aggregateId);
            var task = new SampleRequestTask<RequestMock, CredentialMock, ResourceProviderMock>(
                clientFactoryMock,
                commandSenderMock, 
                eventPublisherMock,
                oldCredentials,
                taskMock,
                aggregateId,
                string.Empty);

            taskMock.Returns(() => System.Threading.Tasks.Task.Run(() => task.Handle(@event)));

            await task.Execute();

            Assert.NotEqual(oldCredentials.Id, newCredentials.Id);

            var tokenParameter = taskMock.GetLastObject<CredentialMock>();
            Assert.Equal(oldCredentials.Id, tokenParameter.Id);

            var actualToken = clientFactoryMock.LastToken as CredentialMock;
            Assert.Equal(newCredentials.Id, actualToken.Id);
        }

        [Fact]
        public void HandlingTokenUpdatedEventWithCorrectServiceUpdatesToken()
        {
            var requestMock =
                new AuthenticatedClientMock<OAuth2Credentials>()
                .SetReturnValue(
                    new List<Tuple<DateTimeOffset, JObject>>
                    {
                        new Tuple<DateTimeOffset, JObject>(DateTimeOffset.Now, new JObject())
                    });
            var clientFactoryMock = new ClientFactoryMock().SetReturnClient<RequestMock>(requestMock);

            var aggregateId = Guid.NewGuid();

            var commandSenderMock = new CommandSenderMock();
            var eventPublisherMock = new EventPublisherMock();

            var expectedJson = new JObject
            {
                ["access_token"] = "someTokenName"
            };
            var expected = expectedJson.ToObject<OAuth2Credentials>();
            var @event = new TokenUpdated<ResourceProviderMock>(expectedJson);
            var authenticationTokenJson = new JObject
            {
                ["access_token"] = "anotherTokenName"
            };
            var authenticationToken = authenticationTokenJson.ToObject<OAuth2Credentials>();

            var taskMock = new TaskMock();

            var task = new SampleRequestTask<RequestMock, OAuth2Credentials, ResourceProviderMock>(
                clientFactoryMock,
                commandSenderMock,
                eventPublisherMock,
                authenticationToken,
                taskMock,
                aggregateId,
                string.Empty);

            task.Handle(@event);

            var actual = task.GetMemberValue<OAuth2Credentials>("_credentials");
            
            Assert.Equal(expected.AccessToken, actual.AccessToken);
        }
    }
}
