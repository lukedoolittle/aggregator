using System;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Contracts
{
    public interface IAuthorizationUISelector
    {
        AuthorizationInterface GetOptimalOAuth1Interface<TResourceProvider>(
            TResourceProvider provider,
            AuthorizationInterface selectedInterface)
            where TResourceProvider : OAuth1ResourceProvider;

        AuthorizationInterface GetOptimalOAuth2Interface<TResourceProvider>(
            TResourceProvider provider,
            AuthorizationInterface selectedInterface,
            Uri callbackUri)
            where TResourceProvider : OAuth2ResourceProvider;
    }
}