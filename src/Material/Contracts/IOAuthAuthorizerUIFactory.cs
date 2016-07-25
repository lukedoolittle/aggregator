using Material.Enums;
using Material.Infrastructure;
using Material.OAuth;

namespace Material.Contracts
{
    public interface IOAuthAuthorizerUIFactory
    {
        IOAuthAuthorizerUI GetAuthorizer<TService>(
            AuthenticationInterfaceEnum browserType,
            OAuthCallbackHandler callbackHandler)
            where TService : ResourceProvider;
    }
}
