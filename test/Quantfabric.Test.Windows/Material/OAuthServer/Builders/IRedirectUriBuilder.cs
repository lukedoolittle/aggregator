using System;
using Material.Domain.Credentials;
using System.Collections.Generic;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public interface IRedirectUriBuilder<TCredentials>
        where TCredentials : TokenCredentials
    {
        Uri BuildRedirectUri(
            Uri redirectUri,
            TCredentials credentials, 
            Dictionary<string, string> additionalParameters);
    }
}
