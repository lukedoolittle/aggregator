using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Framework;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class ProtocolAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials, object>
        where TCredentials : TokenCredentials
    {
        public ProtocolAuthorizerUI(
            IOAuthCallbackHandler<TCredentials> callbackHandler,
            Uri callbackUri,
            AuthorizationInterface @interface,
            Action<Action> runOnMainThread,
            Func<bool> isOnline) : 
                base(
                    callbackHandler, 
                    callbackUri,
                    @interface, 
                    runOnMainThread,
                    isOnline)
        { }

        //TODO: how do we determine the user closed the browser? a timer?
        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, object, bool> callbackHandler)
        {
            Platform.Current.ProtocolLaunch += (s, e) =>
            {
                callbackHandler(e.Uri, null);
            };

            Platform.Current.Launch(authorizationUri);
        }

        protected override void CleanupView(object view)
        { }
    }
}
