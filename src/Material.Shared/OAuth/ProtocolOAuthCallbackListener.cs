using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Framework;

namespace Material.Infrastructure.OAuth
{
    public class ProtocolOAuthCallbackListener : IOAuthCallbackListener
    {
        private readonly IOAuthCallbackHandler _handler;

        public ProtocolOAuthCallbackListener(IOAuthCallbackHandler handler)
        {
            _handler = handler;
        }

        public void Listen<TToken>(
            Uri callbackUri, 
            TaskCompletionSource<TToken> completionSource) 
            where TToken : TokenCredentials
        {
            Platform.Current.ProtocolLaunch += (s, e) =>
            {
                if (e.Uri.ToString().Contains(callbackUri.ToString()))
                {
                    try
                    {
                        var result = _handler
                            .ParseAndValidateCallback<TToken>(
                                e.Uri);
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
