using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class ProtocolOAuthCallbackListener<TCredentials> : 
        IOAuthCallbackListener<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthCallbackHandler<TCredentials> _handler;
        private readonly IProtocolLauncher _launcher;

        public ProtocolOAuthCallbackListener(
            IOAuthCallbackHandler<TCredentials> handler, 
            IProtocolLauncher launcher)
        {
            _handler = handler;
            _launcher = launcher;
        }

        public void Listen(
            Uri callbackUri, 
            string userId,
            TaskCompletionSource<TCredentials> completionSource) 
        {
            _launcher.ProtocolLaunch += (s, e) =>
            {
                if (e.Uri.ToString().Contains(callbackUri.ToString()))
                {
                    try
                    {
                        var result = _handler
                            .ParseAndValidateCallback(
                                e.Uri,
                                userId);
                        if (result != null)
                        {
                            completionSource.SetResult(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        completionSource.SetException(ex);
                        throw;
                    }
                }
            };
        }
    }
}
