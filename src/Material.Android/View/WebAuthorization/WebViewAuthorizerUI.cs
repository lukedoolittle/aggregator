using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Webkit;
using Foundations.Enums;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Framework;
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
        { }

        protected override void CleanupView(object view)
        {
            var webView = (WebView)view;
            webView.StopLoading();
            webView.LoadData(
                StringResources.OAuthCallbackResponse,
                MediaType.Text.EnumToString(),
                string.Empty);

            Platform.Current.Context.Finish();
        }

        protected override async Task MakeAuthorizationRequest(
            Uri authorizationUri,
            Func<Uri, object, bool> callbackHandler)
        {
            var webViewCompletionSource = new TaskCompletionSource<WebViewActivity>();
            var context = Platform.Current.Context;

            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(
                WebViewActivity.Authorizer,
                WebViewActivity.StateRepo.Add(webViewCompletionSource));
            context.StartActivity(intent);

            var activity = await webViewCompletionSource
                .Task
                .ConfigureAwait(true);

            activity.View.SetWebViewClient(
                new AuthorizingWebViewClient((view, url, favicon) =>
                {
                    callbackHandler(new Uri(url), activity.View);
                }));

            activity.View.LoadUrl(authorizationUri.ToString());
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