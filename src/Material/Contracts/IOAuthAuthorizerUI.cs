using System;
using Material.Domain.Credentials;
using System.Threading.Tasks;
using Material.Framework.Enums;

namespace Material.Contracts
{
    public interface IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        AuthorizationInterface BrowserType { get; }

        Task<TCredentials> Authorize(
            Uri authorizationUri,
            string requestId);
    }
}
