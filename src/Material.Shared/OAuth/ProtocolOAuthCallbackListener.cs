using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Framework;

namespace Material.Infrastructure.OAuth
{
    public class ProtocolOAuthCallbackListener<TCredentials> : 
        IOAuthCallbackListener<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthCallbackHandler<TCredentials> _handler;

        public ProtocolOAuthCallbackListener(
            IOAuthCallbackHandler<TCredentials> handler)
        {
            _handler = handler;
        }

        public void Listen(
            Uri callbackUri, 
            string userId,
            TaskCompletionSource<TCredentials> completionSource) 
        {
            Platform.Current.ProtocolLaunch += (s, e) =>
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
                    }
                }
            };
        }
    }
}
