using LightMock;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Material.Infrastructure;

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
                a => a.CreateClient<TRequest>()).Returns(client);

            return this;
        }

        public ClientFactoryMock SetReturnClientDontCareAboutCredentials<TRequest, TCredentials>
            (IRequestClient client)
            where TRequest : Request
        {
            _context.Arrange(
                a => a.CreateClient<TRequest, TCredentials>(
                    The<TCredentials>.IsAnyValue))
                    .Returns(client);

            return this;
        }

        public ClientFactoryMock SetReturnClient<TRequest, TCredentials>(
            IRequestClient client, 
            TCredentials credentials)
            where TRequest : Request
        {
            _context.Arrange(
                a => a.CreateClient<TRequest, TCredentials>(
                    The<TCredentials>.Is(o => o.Equals(credentials))))
                    .Returns(client);

            return this;
        }

        public IRequestClient CreateClient<TRequest>() where TRequest : Request
        {
            var result = _invoker.Invoke(a => a.CreateClient<TRequest>());
            return result;
        }

        public IRequestClient CreateClient<TRequest, TCredentials>(TCredentials credentials) where TRequest : Request
        {
            LastToken = credentials;
            var result = _invoker.Invoke(a => a.CreateClient<TRequest, TCredentials>(credentials));
            return result;
        }
    }
}
