using System;
using System.Threading.Tasks;
using Foundation;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using UIKit;
using Material.Framework;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class UIWebViewAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials>
        where TCredentials : TokenCredentials
    {
        public UIWebViewAuthorizerUI(
            IOAuthCallbackHandler<TCredentials> handler, 
            Uri callbackUri,
            AuthorizationInterface @interface,
            Action<Action> runOnMainThread,
            Func<bool> isOnline) : 
                base(
                    handler, 
                    callbackUri,
                    @interface,
                    runOnMainThread,
                    isOnline)
        { }

        protected override void CleanupView(object view)
        {
            var webView = (UIWebView)view;
            using (var url = new NSUrl("/"))
            {
                webView.LoadHtmlString(
                    StringResources.OAuthCallbackResponse, 
                    url);
            }

            Platform.Current.Context.DismissViewController(false, null);
        }

        protected override async Task MakeAuthorizationRequest(
            Uri authorizationUri,
            Func<Uri, object, bool> callbackHandler)
        {
            var webViewCompletionSource = new TaskCompletionSource<UIWebView>();
            var context = Platform.Current.Context;

            var controller = new WebViewController(
                context.View.Bounds,
                webViewCompletionSource);
            context.PresentViewController(controller, false, null);

            var webView = await webViewCompletionSource
                .Task
                .ConfigureAwait(true);

            webView.ShouldStartLoad = (view, request, type) =>
            {
                var success = callbackHandler(
                    new Uri(request.Url.ToString()), 
                    webView);

                return !success;
            };

            //NSUrlRequest will not handle spaces so ensure URL encoding of spaces
            webView.LoadRequest(
                new NSUrlRequest(
                    new NSUrl(authorizationUri
                        .ToString()
                        .Replace(" ", "%20"))));
        }
    }
}
