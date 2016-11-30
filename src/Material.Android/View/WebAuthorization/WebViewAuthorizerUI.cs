using System;
using System.Threading.Tasks;
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
        OAuthAuthorizationUITemplateBase<TCredentials, WebViewDialog>
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

        protected override void CleanupView(WebViewDialog view)
        {
            view.WebView.StopLoading();
            view.WebView.LoadData(
                StringResources.OAuthCallbackResponse,
                MediaType.Text.EnumToString(),
                string.Empty);

            view.Hide();
        }

        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, WebViewDialog, bool> callbackHandler)
        {
            new WebViewDialog(
                    Resource.Layout.WebViewDialog, 
                    Resource.Id.dialogWebView, 
                    Resource.Id.closeView, 
                    Platform.Current.Context, 
                    credentialsCompletion.SetCanceled)
                .Show(
                    authorizationUri, 
                    callbackHandler);
        }
    }
}