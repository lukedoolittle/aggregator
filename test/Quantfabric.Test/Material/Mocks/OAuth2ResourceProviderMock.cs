using System;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Quantfabric.Test.Integration
{
    public class OAuth2ResourceProviderMock : OAuth2ResourceProvider
    {
        private readonly OAuth2ResourceProvider _provider;

        public int Port { get; } = TestUtilities.GetAvailablePort(30000);

        public OAuth2ResourceProviderMock(
            OAuth2ResourceProvider provider)
        {
            _provider = provider;
        }

        public override List<string> AvailableScopes => _provider.AvailableScopes;
        public override List<OAuth2ResponseType> Flows => _provider.Flows;
        public override List<GrantType> GrantTypes => _provider.GrantTypes;

        public override Uri AuthorizationUrl => _provider.AuthorizationUrl != null ? 
            new Uri($"http://localhost:{Port}{_provider.AuthorizationUrl.AbsolutePath}") : 
            null;

        public override Uri TokenUrl => _provider.TokenUrl != null ?
            new Uri($"http://localhost:{Port}{_provider.TokenUrl.AbsolutePath}") :
            null;
    }
}
