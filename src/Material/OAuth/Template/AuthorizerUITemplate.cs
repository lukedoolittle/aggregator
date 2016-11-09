using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{
    //TODO: add other methods here to abstract out the duplicate behavior
    //in ProtocolIOAuthCallbackListener, HttpOAuthCallbackListener, WebViewAuthorizerUI,
    //UIWebViewAuthorizerUI, and WebViewAuthorizerUI
    public abstract class AuthorizerUITemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthCallbackHandler<TCredentials> _handler;

        protected AuthorizerUITemplate(
            IOAuthCallbackHandler<TCredentials> handler)
        {
            _handler = handler;
        }

        protected void RespondToUri(
            Uri uri, 
            string userId,
            TaskCompletionSource<TCredentials> completionSource,
            Action success)
        {
            try
            {
                var result = _handler
                    .ParseAndValidateCallback(
                        uri,
                        userId);
                if (result != null)
                {
                    completionSource.SetResult(result);
                    success();
                }
            }
            catch (Exception ex)
            {
                completionSource.SetException(ex);
                throw;
            }
        }
    }
}
