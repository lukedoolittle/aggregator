using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Webkit;
using Foundations.Enums;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using Material.Framework;
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
            var completionSource = new TaskCompletionSource<TCredentials>();
            var webViewCompletionSource = new TaskCompletionSource<WebViewActivity>();
            var context = Platform.Current.Context;

            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(
                WebViewActivity.Authorizer, 
                WebViewActivity.StateRepo.Add(webViewCompletionSource));
            context.StartActivity(intent);
            
            var activity = await webViewCompletionSource.Task.ConfigureAwait(false);
            Platform.Current.RunOnMainThread(() =>
            {
                activity.View.SetWebViewClient(
                    new AuthorizingWebViewClient((view, url, favicon) =>
                    {
                        if (url.StartsWith(_callbackUri.AbsoluteUri))
                        {
                            view.StopLoading();
                            view.LoadData(
                                StringResources.OAuthCallbackResponse,
                                MediaType.Text.EnumToString(),
                                string.Empty);

                            RespondToUri(
                                new Uri(url),
                                userId,
                                completionSource, 
                                () => activity.Finish());
                        }
                    }));

                if (!Platform.Current.IsOnline)
                {
                    throw new NoConnectivityException(
                        StringResources.OfflineConnectivityException);
                }

                activity.View.LoadUrl(authorizationUri.ToString());
            });

            return await completionSource.Task.ConfigureAwait(false);
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