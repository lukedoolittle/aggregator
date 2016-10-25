using System;
using System.Threading.Tasks;
using Foundation;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using UIKit;
using Material.Framework;

namespace Material.View.WebAuthorization
{
    public class UIWebViewAuthorizerUI<TCredentials> : 
        IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly Uri _callbackUri;
        private readonly IOAuthCallbackHandler<TCredentials> _handler;

        public AuthenticationInterface BrowserType => 
            AuthenticationInterface.Embedded;

        public UIWebViewAuthorizerUI(
            IOAuthCallbackHandler<TCredentials> handler, 
            Uri callbackUri)
        {
            _handler = handler;
            _callbackUri = callbackUri;
        }

        public async Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId)
        {
            var taskCompletionSource = new TaskCompletionSource<TCredentials>();
            var webViewCompletionSource = new TaskCompletionSource<UIWebView>();

            Platform.Current.RunOnMainThread(async () =>
            {
                var context = Platform.Current.Context;

                var controller = new WebViewController(
                    context.View.Bounds,
                    webViewCompletionSource);
                context.PresentViewController(controller, false, null);

                var webView = await webViewCompletionSource.Task.ConfigureAwait(false);

                if (!Platform.Current.IsOnline)
                {
                    throw new NoConnectivityException(
                        StringResources.OfflineConnectivityException);
                }
                webView.LoadRequest(new NSUrlRequest(new NSUrl(authorizationUri.ToString())));
                webView.ShouldStartLoad = (view, request, type) =>
                {
                    if (request.Url.ToString().StartsWith(
                        _callbackUri.ToString()))
                    {
                        webView.LoadHtmlString(
                            StringResources.OAuthCallbackResponse,
                            new NSUrl("/"));

                        var result = _handler
                            .ParseAndValidateCallback(
                                new Uri(request.Url.ToString()),
                                userId);
                        taskCompletionSource.SetResult(result);

                        controller.DismissViewController(false, null);
                        return false;
                    }

                    return true;
                };
            });

            return await taskCompletionSource.Task.ConfigureAwait(false);
        }
    }
}
