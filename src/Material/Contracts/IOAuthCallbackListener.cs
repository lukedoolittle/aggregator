using System;
using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthCallbackListener<TCredentials>
        where TCredentials : TokenCredentials
    {
        void Listen(
            Uri callbackUri,
            string userId,
            TaskCompletionSource<TCredentials> completionSource);
    }
}
