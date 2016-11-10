using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class WebViewAuthorizerUI<TCredentials> : 
        OAuthAuthorizationUITemplateBase<TCredentials>
        where TCredentials : TokenCredentials
    {
        public WebViewAuthorizerUI(
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
        {}

        protected override void CleanupView(object view)
        {
            var webView = (WebView)view;
            webView.NavigateToString(StringResources.OAuthCallbackResponse);

            Platform.Current.Context.GoBack();
        }

        protected override async Task MakeAuthorizationRequest(
            Uri authorizationUri,
            Func<Uri, object, bool> callbackHandler)
        {
            var viewCompletionSource = new TaskCompletionSource<WebView>();
            Platform.Current.Context.Navigate(
                typeof(WebViewPage),
                viewCompletionSource);

            var webView = await viewCompletionSource
                .Task
                .ConfigureAwait(false);

            webView.NavigationStarting += (sender, args) =>
            {
                callbackHandler(args.Uri, webView);
            };

            webView.Navigate(authorizationUri);
        }
    }
}
