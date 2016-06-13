using System;
using System.Threading.Tasks;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Framework.Contracts
{
    public interface IWebAuthorizer
    {
        AuthenticationInterfaceEnum BrowserType { get; }

        Task<TToken> Authorize<TToken>(
            Uri callbackUri,
            Uri authorizationUri)
            where TToken : TokenCredentials;
    }
}
