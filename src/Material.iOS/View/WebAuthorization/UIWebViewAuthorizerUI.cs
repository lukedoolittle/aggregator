using System;
using System.Threading.Tasks;
using Foundation;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Framework;
using Material.Framework.Enums;
using Material.Workflow.Template;

namespace Material.View.WebAuthorization
{
    public class UIWebViewAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials, WebDialogView>
        where TCredentials : TokenCredentials
    {
        public UIWebViewAuthorizerUI(
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

        protected override void CleanupView(WebDialogView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            using (var url = new NSUrl("/"))
            {
                view.WebView.LoadHtmlString(
                    StringResources.OAuthCallbackResponse, 
                    url);
            }

            view.Hide();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, WebDialogView, bool> callbackHandler)
        {
            WebDialogView.Create(
                    Platform.Current.Context, 
                    credentialsCompletion.SetCanceled)
                .Show(
                    authorizationUri, 
                    callbackHandler);
        }
    }
}
