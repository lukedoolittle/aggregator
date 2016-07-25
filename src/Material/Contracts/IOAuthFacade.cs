using System;
using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthFacade<TCredentials>
        where TCredentials : TokenCredentials
    {
        Uri CallbackUri { get; }

        Task<Uri> GetAuthorizationUri();

        Task<TCredentials> GetAccessTokenFromCallbackResult(TCredentials result);
    }
}
