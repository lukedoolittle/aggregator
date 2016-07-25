using System;
using System.Threading.Tasks;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAuthorizerUI
    {
        AuthenticationInterfaceEnum BrowserType { get; }

        Task<TToken> Authorize<TToken>(
            Uri callbackUri,
            Uri authorizationUri)
            where TToken : TokenCredentials;
    }
}
