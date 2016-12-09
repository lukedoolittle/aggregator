using System;
using System.Threading.Tasks;
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
            view.Hide();
        }

        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, WebViewDialog, bool> callbackHandler)
        {
            new WebViewDialog(
                    Platform.Current.Context, 
                    credentialsCompletion.SetCanceled)
                .Show(
                    authorizationUri, 
                    callbackHandler);
        }
    }
}