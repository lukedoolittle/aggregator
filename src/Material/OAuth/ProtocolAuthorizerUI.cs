using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class ProtocolAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials, object>
        where TCredentials : TokenCredentials
    {
        private readonly IProtocolLauncher _launcher;
        private readonly IBrowser _browser;

        public ProtocolAuthorizerUI(
            IProtocolLauncher launcher,
            IBrowser browser,
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
        {
            _launcher = launcher;
            _browser = browser;
        }

        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, object, bool> callbackHandler)
        {
            _launcher.ProtocolLaunch = (uri) =>
            {
                callbackHandler(uri, null);
            };

            _browser.Launch(authorizationUri);
        }

        protected override void CleanupView(object view)
        { }
    }
}
