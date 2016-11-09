using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class WebViewAuthorizerUI<TCredentials> : 
        AuthorizerUITemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly Uri _callbackUri;

        public WebViewAuthorizerUI(
            IOAuthCallbackHandler<TCredentials> handler, 
            Uri callbackUri,
            AuthorizationInterface @interface,
            Action<Action> runOnMainThread) : 
                base(
                    handler, 
                    callbackUri,
                    @interface,
                    runOnMainThread)
        {
            _callbackUri = callbackUri;
        }

        //TODO: fix this to properly inherit from AuthorizerUITemplate
        public override async Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId)
        {
            var viewCompletionSource = new TaskCompletionSource<WebView>();
            var completionSource = new TaskCompletionSource<TCredentials>();

            Platform.Current.RunOnMainThread(async () =>
            {
                Platform.Current.Context.Navigate(
                    typeof(WebViewPage),
                    viewCompletionSource);

                var webView = await viewCompletionSource
                    .Task
                    .ConfigureAwait(false);

                webView.NavigationStarting += (sender, args) =>
                {
                    if (args.Uri != null &&
                        args.Uri.ToString().StartsWith(
                            _callbackUri.ToString()))
                    {
                        webView.NavigateToString(StringResources.OAuthCallbackResponse);

                        RespondToUri(
                            args.Uri, 
                            userId, 
                            completionSource, 
                            () => Platform.Current.Context.GoBack());
                    }
                };

                if (!Platform.Current.IsOnline)
                {
                    throw new NoConnectivityException(
                        StringResources.OfflineConnectivityException);
                }

                webView.Navigate(authorizationUri);
            });

            return await completionSource.Task.ConfigureAwait(false);
        }
    }
}
