using System;
using LightMock;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;

namespace Aggregator.Test.Mocks
{
    public class ClientFactoryMock : MockBase<IClientFactory>, IClientFactory
    {
        public object LastToken { get; private set; }

        public ClientFactoryMock SetReturnClient<TRequest>
            (IRequestClient client)
            where TRequest : Request
        {
            _context.Arrange(
                a => a.CreateClient(The<Type>.Is(t => t == typeof(TRequest))))
                    .Returns(client);

            return this;
        }

        public ClientFactoryMock SetReturnClientDontCareAboutCredentials<TRequest>
            (IRequestClient client)
            where TRequest : Request
        {
            _context.Arrange(
                a => a.CreateClient(
                    The<Type>.Is(t => t == typeof(TRequest)),
                    The<object>.IsAnyValue))
                    .Returns(client);

            return this;
        }

        public ClientFactoryMock SetReturnClient<TRequest>(
            IRequestClient client, 
            object credentials)
            where TRequest : Request
        {
            _context.Arrange(
                a => a.CreateClient(
                    The<Type>.Is(t => t == typeof(TRequest)),
                    The<object>.Is(o => o == credentials)))
                    .Returns(client);

            return this;
        }

        public IRequestClient CreateClient(Type requestType)
        {
            var result = _invoker.Invoke(a => a.CreateClient(requestType));
            return result;
        }

        public IRequestClient CreateClient<TRequest>(object credentials) where TRequest : Request
        {
            return CreateClient(typeof (TRequest), credentials);
        }

        public IRequestClient CreateClient(Type requestType, object credentials)
        {
            LastToken = credentials;
            var result = _invoker.Invoke(a => a.CreateClient(requestType, credentials));
            return result;
        }
    }
}
