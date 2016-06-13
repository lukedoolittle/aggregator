using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Webkit;
using Aggregator.Framework;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Exceptions;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.View.WebAuthorization
{
    public class WebViewWebAuthorizer : WebAuthorizerBase, IWebAuthorizer
    {
        private readonly Context _context;

        public AuthenticationInterfaceEnum BrowserType => 
            AuthenticationInterfaceEnum.Embedded;

        public WebViewWebAuthorizer(Context context)
        {
            _context = context;
        }

        public async Task<TToken> Authorize<TToken>(
            Uri callbackUri,
            Uri authorizationUri)
            where TToken : TokenCredentials
        {
            var taskCompletion = new TaskCompletionSource<TToken>();
            var webViewCompletionSource = new TaskCompletionSource<WebView>();

            var intent = new Intent(_context, typeof(WebViewActivity));
            //TODO: remove magic strings
            intent.PutExtra(
                "Authorizer", 
                WebViewActivity.StateRepo.Add(webViewCompletionSource));
            _context.StartActivity(intent);
            
            var webView = await webViewCompletionSource.Task.ConfigureAwait(false);
            webView.SetWebViewClient(
                new AuthorizingWebViewClient((view, url, favicon) =>
                {
                    if (url.Contains(callbackUri.AbsoluteUri))
                    {
                        view.LoadData(
                            _responseText,
                            _responseType.ToString(),
                            string.Empty);
                        
                        HandleResponse(taskCompletion, new Uri(url));
                    }
                }));

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException();
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