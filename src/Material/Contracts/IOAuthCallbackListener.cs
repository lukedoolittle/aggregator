using System;
using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthCallbackListener
    {
        void Listen<TToken>(
            Uri callbackUri,
            TaskCompletionSource<TToken> completionSource)
            where TToken : TokenCredentials;
    }
}
