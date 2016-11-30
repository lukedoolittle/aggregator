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
        OAuthAuthorizationUITemplateBase<TCredentials, UIWebView>
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

        protected override void CleanupView(UIWebView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            using (var url = new NSUrl("/"))
            {
                view.LoadHtmlString(
                    StringResources.OAuthCallbackResponse, 
                    url);
            }

            Platform.Current.Context.DismissViewController(false, null);
        }

        protected override async void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, UIWebView, bool> callbackHandler)
        {
            var webViewCompletionSource = new TaskCompletionSource<UIWebView>();
            var context = Platform.Current.Context;

            var controller = new WebViewController(
                context.View.Bounds,
                webViewCompletionSource);
            context.PresentViewController(controller, false, null);

            //TODO: why do we need to wait here, cant we just pass values
            //necessary to complete this into the constructor???
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

            //TODO: determine why spaces are not properly Url encoded
            webView.LoadRequest(
                new NSUrlRequest(
                    new NSUrl(authorizationUri
                        .ToString().Replace(" ", "%20"))));
        }
    }
}
