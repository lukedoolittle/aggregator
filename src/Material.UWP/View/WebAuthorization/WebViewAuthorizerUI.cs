using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class WebViewAuthorizerUI<TCredentials> : 
        OAuthAuthorizationUITemplateBase<TCredentials, WebDialogControl>
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
        {}

        protected override void CleanupView(WebDialogControl view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            view.WebView.NavigateToString(StringResources.OAuthCallbackResponse);

            view.Hide();
        }

        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, WebDialogControl, bool> callbackHandler)
        {
            new WebDialogControl(
                    Window.Current, 
                    credentialsCompletion.SetCanceled)
                .Show(
                    authorizationUri, 
                    callbackHandler);
        }
    }
}
