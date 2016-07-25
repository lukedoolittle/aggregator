using System;
using SimpleCQRS.Autofac.Test.Eventing;
using Xunit;

namespace Aggregator.Test.Integration
{
    using System.Threading.Tasks;

    public class MessageBusTests : IClassFixture<MessageBusFixture>
    {
        private readonly MessageBusFixture _fixture;

        public MessageBusTests(MessageBusFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PassOpenSubscriptionValidEventHandlerAndMessage()
        {
            object actual = null;
            EventHandlerMock<GenericDerived1>.Action = new Action<object>(o => actual = o);
            var messageBus = _fixture.Bus;

            var expected = new EventMock<GenericDerived1>();

            await messageBus.Publish(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task PassOpenSubscriptionValidNonGenericEventHandlerAndMessage()
        {
            object actual = null;
            EventHandlerMock1.Action = new Action<object>(o => actual = o);
            var messageBus = _fixture.Bus;

            var expected = new NonGenericEventMock();

            await messageBus.Publish(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task PassOpenSubscriptionValidCommandHandlerAndMessage()
        {
            object actual = null;
            CommandHandlerMock<GenericDerived1>.Action = new Action<object>(o => actual = o);
            var messageBus = _fixture.Bus;

            var expected = new CommandMock<GenericDerived1>();

            await messageBus.Send(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task PassOpenSubscriptionValidNonGenericCommandHandlerAndMessage()
        {
            object actual = null;
            CommandHandlerMock1.Action = new Action<object>(o => actual = o);
            var messageBus = _fixture.Bus;

            var expected = new NonGenericCommandMock();

            await messageBus.Send(expected);

            Assert.Equal(expected, actual);
        }


    }
}
