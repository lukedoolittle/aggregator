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
        OAuthAuthorizationUITemplateBase<TCredentials>
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

        protected override Task MakeAuthorizationRequest(
            Uri authorizationUri, 
            Func<Uri, object, bool> callbackHandler)
        {
            Platform.Current.ProtocolLaunch += (s, e) =>
            {
                callbackHandler(e.Uri, null);
            };

            Platform.Current.Launch(authorizationUri);

            return Task.FromResult(true);
        }

        protected override void CleanupView(object view)
        { }
    }
}
