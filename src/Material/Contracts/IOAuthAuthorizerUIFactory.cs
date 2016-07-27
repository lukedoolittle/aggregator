using Material.Enums;
using Material.Infrastructure;

namespace Material.Contracts
{
    public interface IOAuthAuthorizerUIFactory
    {
        IOAuthAuthorizerUI GetAuthorizer<TService>(
            AuthenticationInterfaceEnum browserType,
            IOAuthCallbackHandler callbackHandler)
            where TService : ResourceProvider;
    }
}
