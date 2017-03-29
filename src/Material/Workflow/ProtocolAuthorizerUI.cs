using System;
using Material.Domain.Credentials;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Workflow.Template;

namespace Material.Workflow
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
