using System;
using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthFacade<TCredentials> : 
        IOAuthCallbackHandler<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<Uri> GetAuthorizationUriAsync(string userId);

        Task<TCredentials> GetAccessTokenAsync(
            TCredentials intermediateResult,
            string secret);
    }
}
