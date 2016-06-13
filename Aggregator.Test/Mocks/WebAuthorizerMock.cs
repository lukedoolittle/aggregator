using System;
using LightMock;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Test.Helpers.Mocks;

namespace Aggregator.Test.Mocks
{
    using System.Threading.Tasks;

    public class WebAuthorizerMock : MockBase<IWebAuthorizer>, IWebAuthorizer
    {
        public AuthenticationInterfaceEnum BrowserType { get; }

        public WebAuthorizerMock SetReturnToken<TToken>(TToken tokenToReturn)
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
