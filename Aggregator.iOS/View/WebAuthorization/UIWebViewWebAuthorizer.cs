using System;
using System.Threading.Tasks;
using Foundation;
using Aggregator.Framework;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Exceptions;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Credentials;
using UIKit;

namespace Aggregator.View.WebAuthorization
{
    public class UIWebViewWebAuthorizer : WebAuthorizerBase, IWebAuthorizer
    {
        private readonly UIViewController _context;

        public AuthenticationInterfaceEnum BrowserType => 
            AuthenticationInterfaceEnum.Embedded;

        public UIWebViewWebAuthorizer(UIViewController context)
        {
            _context = context;
        }

        public async Task<TToken> Authorize<TToken>(
            Uri callbackUri, 
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var taskCompletionSource = new TaskCompletionSource<TToken>();
            var webViewCompletionSource = new TaskCompletionSource<UIWebView>();

            var controller = new WebViewController(
                _context.View.Bounds, 
                webViewCompletionSource);
            _context.PresentViewController(controller, false, null);

            var webView = await webViewCompletionSource.Task.ConfigureAwait(false);

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException();
            }
            webView.LoadRequest(new NSUrlRequest(new NSUrl(authorizationUri.ToString())));
            webView.ShouldStartLoad = (view, request, type) =>
            {
                if (request.Url.ToString().Contains(callbackUri.ToString()))
                {
                    webView.LoadHtmlString(
                        _responseText,
                        new NSUrl("/"));

                    HandleResponse(taskCompletionSource, new Uri(request.Url.ToString()));

                    controller.DismissViewController(false, null);
                    return false;
                }

                return true;
            };

            return await taskCompletionSource.Task.ConfigureAwait(false);
        }
    }
}
