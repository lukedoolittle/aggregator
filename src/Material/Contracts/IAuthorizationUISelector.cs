using System;
using Material.Domain.Core;
using Material.Framework.Enums;

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