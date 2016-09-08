using System;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAuthorizerUIFactory
    {
        IOAuthAuthorizerUI<TCredentials> GetAuthorizer<TService, TCredentials>(
            AuthenticationInterfaceEnum browserType,
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri)
            where TService : ResourceProvider
            where TCredentials : TokenCredentials;
    }
}
