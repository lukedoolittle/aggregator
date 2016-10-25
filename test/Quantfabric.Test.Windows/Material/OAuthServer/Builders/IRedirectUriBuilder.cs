using System;
using System.Collections.Generic;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.OAuthServer.Builders
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
