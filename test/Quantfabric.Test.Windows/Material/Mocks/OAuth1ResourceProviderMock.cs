using System;
using Material.Domain.Core;
using Material.Framework.Enums;

namespace Quantfabric.Test.Material.Mocks
{
    public class OAuth1ResourceProviderMock : OAuth1ResourceProvider
    {
        private readonly OAuth1ResourceProvider _provider;

        public int Port { get; } = TestUtilities.GetAvailablePort(30000);

        public OAuth1ResourceProviderMock(
            OAuth1ResourceProvider provider)
        {
            _provider = provider;
        }

        public override Uri RequestUrl => _provider.RequestUrl != null ?
            new Uri($"http://localhost:{Port}{_provider.RequestUrl.AbsolutePath}") :
            null;

        public override Uri AuthorizationUrl => _provider.AuthorizationUrl != null ?
            new Uri($"http://localhost:{Port}{_provider.AuthorizationUrl.AbsolutePath}") :
            null;

        public override Uri TokenUrl => _provider.TokenUrl != null ?
            new Uri($"http://localhost:{Port}{_provider.TokenUrl.AbsolutePath}") :
            null;

        public override HttpParameterType ParameterType => _provider.ParameterType;
    }
}
