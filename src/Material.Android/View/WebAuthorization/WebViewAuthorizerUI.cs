using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Webkit;
using Foundations.Extensions;
using Foundations.Http;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using Material.Framework;

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
            var taskCompletion = new TaskCompletionSource<TToken>();
            var webViewCompletionSource = new TaskCompletionSource<WebView>();
            var context = Platform.Context;

            var intent = new Intent(context, typeof(WebViewActivity));
            //TODO: remove magic strings
            intent.PutExtra(
                "Authorizer", 
                WebViewActivity.StateRepo.Add(webViewCompletionSource));
            context.StartActivity(intent);
            
            var webView = await webViewCompletionSource.Task.ConfigureAwait(false);
            webView.SetWebViewClient(
                new AuthorizingWebViewClient((view, url, favicon) =>
                {
                    if (url.Contains(callbackUri.AbsoluteUri))
                    {
                        view.LoadData(
                            StringResources.OAuthCallbackResponse,
                            MimeTypeEnum.Text.EnumToString(),
                            string.Empty);
                        
                        var result = _handler
                            .ParseAndValidateCallback<TToken>(
                                new Uri(url));
                        taskCompletion.SetResult(result);
                    }
                }));

            if (!Platform.IsOnline)
            {
                throw new NoConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            webView.LoadUrl(authorizationUri.ToString());

            return await taskCompletion.Task.ConfigureAwait(false);
        }

        private class AuthorizingWebViewClient : WebViewClient
        {
            private readonly Action<WebView, string, Bitmap> _pageLoadAction;

            public AuthorizingWebViewClient(Action<WebView, string, Bitmap> pageLoadAction)
            {
                _pageLoadAction = pageLoadAction;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                return false;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                _pageLoadAction(view, url, favicon);
            }
        }
    }
}