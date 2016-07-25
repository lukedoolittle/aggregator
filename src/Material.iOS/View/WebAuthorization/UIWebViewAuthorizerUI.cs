using System;
using System.Threading.Tasks;
using Material;
using Foundation;
using Aggregator.Framework;
using Aggregator.Infrastructure;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using Material.OAuth;
using UIKit;

namespace Aggregator.View.WebAuthorization
{
    public class UIWebViewAuthorizerUI : IOAuthAuthorizerUI
    {
        private readonly OAuthCallbackHandler _handler;

        public AuthenticationInterfaceEnum BrowserType => 
            AuthenticationInterfaceEnum.Embedded;

        public UIWebViewAuthorizerUI(OAuthCallbackHandler handler)
        {
            _handler = handler;
        }

        public async Task<TToken> Authorize<TToken>(
            Uri callbackUri, 
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var taskCompletionSource = new TaskCompletionSource<TToken>();
            var webViewCompletionSource = new TaskCompletionSource<UIWebView>();
            var context = Platform.Context;

            var controller = new WebViewController(
                context.View.Bounds, 
                webViewCompletionSource);
            context.PresentViewController(controller, false, null);

            var webView = await webViewCompletionSource.Task.ConfigureAwait(false);

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException(
                    StringResources.OfflineConnectivityException);
            }
            webView.LoadRequest(new NSUrlRequest(new NSUrl(authorizationUri.ToString())));
            webView.ShouldStartLoad = (view, request, type) =>
            {
                if (request.Url.ToString().Contains(callbackUri.ToString()))
                {
                    webView.LoadHtmlString(
                        StringResources.OAuthCallbackResponse,
                        new NSUrl("/"));

                    var result = _handler
                        .ParseAndValidateCallback<TToken>(
                            new Uri(request.Url.ToString()));
                    taskCompletionSource.SetResult(result);

                    controller.DismissViewController(false, null);
                    return false;
                }

                return true;
            };

            return await taskCompletionSource.Task.ConfigureAwait(false);
        }

        private static UIViewController GetCurrentController()
        {
            var viewController = UIApplication
                .SharedApplication
                .KeyWindow
                .RootViewController;
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}
