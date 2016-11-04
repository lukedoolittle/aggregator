using System;
using System.Collections.Generic;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.OAuthServer.Builders;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth1AccessTokenCredentialBuilder : 
        ICredentialBuilder<OAuth1Credentials, OAuth1Request>
    {
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;

        public OAuth1AccessTokenCredentialBuilder(
            IDictionary<string, List<OAuth1Token>> tokens)
        {
            _tokens = tokens;
        }

        public OAuth1Credentials BuildCredentials(OAuth1Request request)
        {
            throw new NotImplementedException("Build final OAuth1 credentials");
        }
    }
}
