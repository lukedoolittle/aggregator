using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;

namespace Aggregator.Framework.Contracts
{
    public interface IWebAuthorizerFactory
    {
        IWebAuthorizer GetAuthorizer<TService>(
            object context, 
            AuthenticationInterfaceEnum browserType)
            where TService : Service;
    }
}
