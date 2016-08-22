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
    public class WebViewAuthorizerUI : IOAuthAuthorizerUI
    {
        private readonly IOAuthCallbackHandler _handler;

        public AuthenticationInterfaceEnum BrowserType =>
            AuthenticationInterfaceEnum.Embedded;

        public WebViewAuthorizerUI(IOAuthCallbackHandler handler)
        {
            _handler = handler;
        }

        public async Task<TToken> Authorize<TToken>(
            Uri callbackUri,
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var viewCompletionSource = new TaskCompletionSource<WebView>();
            var tokenCompletionSource = new TaskCompletionSource<TToken>();

            Platform.RunOnMainThread(async () =>
            {
                Platform.Context.Navigate(
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
                            .ParseAndValidateCallback<TToken>(
                                args.Uri);
                        tokenCompletionSource.SetResult(result);

                        Platform.Context.GoBack();
                    }
                };

                if (!Platform.IsOnline)
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
