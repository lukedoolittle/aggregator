using System;
using System.Threading.Tasks;
using Foundation;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using UIKit;
using Material.Framework;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class UIWebViewAuthorizerUI<TCredentials> :
        AuthorizerUITemplate<TCredentials>,
        IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly Uri _callbackUri;

        public AuthorizationInterface BrowserType => 
            AuthorizationInterface.Embedded;

        public UIWebViewAuthorizerUI(
            IOAuthCallbackHandler<TCredentials> handler, 
            Uri callbackUri) :
                base(handler)
        {
            _callbackUri = callbackUri;
        }

        public async Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId)
        {
            var completionSource = new TaskCompletionSource<TCredentials>();
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

                //NSUrlRequest will not handle spaces so ensure URL encoding
                var endpoint = new NSUrl(authorizationUri
                    .ToString()
                    .Replace(" ", "%20"));
                webView.LoadRequest(new NSUrlRequest(endpoint));
                webView.ShouldStartLoad = (view, request, type) =>
                {
                    if (request.Url.ToString().StartsWith(
                        _callbackUri.ToString()))
                    {
                        webView.LoadHtmlString(
                            StringResources.OAuthCallbackResponse,
                            new NSUrl("/"));

                        RespondToUri(
                            new Uri(request.Url.ToString()), 
                            userId, 
                            completionSource, 
                            () => controller.DismissViewController(false, null));

                        return false;
                    }

                    return true;
                };
            });

            return await completionSource.Task.ConfigureAwait(false);
        }
    }
}
