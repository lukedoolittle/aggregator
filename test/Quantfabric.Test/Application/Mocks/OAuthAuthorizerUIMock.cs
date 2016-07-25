using System;
using LightMock;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Aggregator.Test.Mocks
{
    using System.Threading.Tasks;

    public class OAuthAuthorizerUIMock : MockBase<IOAuthAuthorizerUI>, IOAuthAuthorizerUI
    {
        public AuthenticationInterfaceEnum BrowserType { get; }

        public OAuthAuthorizerUIMock SetReturnToken<TToken>(TToken tokenToReturn)
            where TToken : TokenCredentials
        {
            _context.Arrange(a => 
                a.Authorize<TToken>(
                    The<Uri>.IsAnyValue, 
                    The<Uri>.IsAnyValue))
                .Returns(Task.FromResult(tokenToReturn));

            return this;
        }

        public Task<TToken> Authorize<TToken>(
            Uri callbackUri,
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            return _invoker.Invoke(a => a.Authorize<TToken>(
                callbackUri, 
                authorizationUri));
        }
    }
}
