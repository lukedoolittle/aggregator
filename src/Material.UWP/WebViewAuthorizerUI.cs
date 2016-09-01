using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Credentials;

namespace Material.View.WebAuthorization
{
    public class WebViewAuthorizerUI<TCredentials> : 
        IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthCallbackHandler<TCredentials> _handler;

        public AuthenticationInterfaceEnum BrowserType =>
            AuthenticationInterfaceEnum.Embedded;

        public WebViewAuthorizerUI(IOAuthCallbackHandler<TCredentials> handler)
        {
            _handler = handler;
        }

        //TODO: inject these static platform references
        public async Task<TCredentials> Authorize(
            Uri callbackUri,
            Uri authorizationUri,
            string userId)
        {
            var viewCompletionSource = new TaskCompletionSource<WebView>();
            var tokenCompletionSource = new TaskCompletionSource<TCredentials>();

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
                        args.Uri.ToString().Contains(
                            callbackUri.ToString()))
                    {
                        var cancel = args.Cancel;

                        webView.NavigateToString(StringResources.OAuthCallbackResponse);

                        var result = _handler
                            .ParseAndValidateCallback(
                                args.Uri,
                                userId);
                        tokenCompletionSource.SetResult(result);

                        Platform.Current.Context.GoBack();
                    }
                };

                if (!Platform.Current.IsOnline)
                {
                    throw new NoConnectivityException(
                        StringResources.OfflineConnectivityException);
                }

                webView.Navigate(authorizationUri);
            });

            return await tokenCompletionSource.Task.ConfigureAwait(false);
        }
    }
}
