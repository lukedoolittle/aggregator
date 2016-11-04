using System;
using System.Collections.Generic;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.OAuthServer.Builders;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth1AuthorizationRedirectBuilder : 
        IRedirectUriBuilder<OAuth1Credentials>
    {
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;

        public OAuth1AuthorizationRedirectBuilder(
            IDictionary<string, List<OAuth1Token>> tokens)
        {
            _tokens = tokens;
        }

        public Uri BuildRedirectUri(
            Uri redirectUri, 
            OAuth1Credentials credentials, 
            Dictionary<string, string> additionalParameters)
        {
            throw new NotImplementedException("Build OAuth1 redirect uri");
        }
    }
}
