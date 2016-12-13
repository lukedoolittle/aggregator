using System;
using System.Threading.Tasks;
using Android.App;
using Android.Support.CustomTabs;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.OAuth.Template;

namespace Material.View.WebAuthorization
{
    public class ChromeTabsAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials, CustomTabsActivityManager>
        where TCredentials : TokenCredentials
    {
        private readonly IProtocolLauncher _launcher;
        private readonly Activity _context;

        public ChromeTabsAuthorizerUI(
            IProtocolLauncher launcher,
            Activity context,
            IOAuthCallbackHandler<TCredentials> handler,
            Uri callbackUri,
            AuthorizationInterface browserType,
            Action<Action> runOnMainThread,
            Func<bool> isOnline) :
                base(
                handler,
                callbackUri,
                browserType,
                runOnMainThread,
                isOnline)
        {
            _context = context;
            _launcher = launcher;
        }

        protected override void MakeAuthorizationRequest(
            Uri authorizationUri,
            TaskCompletionSource<TCredentials> credentialsCompletion,
            Func<Uri, CustomTabsActivityManager, bool> callbackHandler)
        {
            var manager = new CustomTabsActivityManager(_context);
            manager.CustomTabsServiceConnected += (name, client) => 
            { 
                manager.LaunchUrl(authorizationUri.ToString());
            };
            manager.CustomTabsServiceDisconnected += name =>
            {
                object a = null;
                credentialsCompletion.SetCanceled();
            };
            _launcher.ProtocolLaunch = (uri) =>
            {
                callbackHandler(uri, manager);
            };

            if (!manager.BindService())
            {
                throw new Exception();
            }
        }

        protected override void CleanupView(
            CustomTabsActivityManager view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            //TODO: how do you dismiss the view???
            //view.DismissViewController(
            //    true,
            //    null,
            //    false);
        }
    }
}