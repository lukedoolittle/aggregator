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
            var webViewCompletionSource = new TaskCompletionSource<WebViewActivity>();
            var context = Platform.Context;

            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(
                WebViewActivity.Authorizer, 
                WebViewActivity.StateRepo.Add(webViewCompletionSource));
            context.StartActivity(intent);
            
            var activity = await webViewCompletionSource.Task.ConfigureAwait(false);
            activity.View.SetWebViewClient(
                new AuthorizingWebViewClient((view, url, favicon) =>
                {
                    if (url.Contains(callbackUri.AbsoluteUri))
                    {
                        view.StopLoading();
                        view.LoadData(
                            StringResources.OAuthCallbackResponse,
                            MediaTypeEnum.Text.EnumToString(),
                            string.Empty);
                        
                        var result = _handler
                            .ParseAndValidateCallback<TToken>(
                                new Uri(url));
                        taskCompletion.SetResult(result);
                        activity.Finish();
                    }
                }));

            if (!Platform.IsOnline)
            {
                throw new NoConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            activity.View.LoadUrl(authorizationUri.ToString());

            return await taskCompletion.Task.ConfigureAwait(false);
        }

        private class AuthorizingWebViewClient : WebViewClient
        {
            private readonly Action<WebView, string, Bitmap> _pageLoadAction;

            public AuthorizingWebViewClient(
                Action<WebView, string, Bitmap> pageLoadAction)
            {
                _pageLoadAction = pageLoadAction;
            }

            public override bool ShouldOverrideUrlLoading(
                WebView view,
                string url)
            {
                return false;
            }

            public override void OnPageStarted(
                WebView view, 
                string url, 
                Bitmap favicon)
            {
                _pageLoadAction(view, url, favicon);
            }
        }
    }
}