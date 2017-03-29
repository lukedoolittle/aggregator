using System;
using System.Threading.Tasks;
using Android.App;
using Android.Support.CustomTabs;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Framework.Enums;
using Material.Workflow.Template;

namespace Material.View.WebAuthorization
{
    public class ChromeTabsAuthorizerUI<TCredentials> :
        OAuthAuthorizationUITemplateBase<TCredentials, CustomTabsActivityManager>
        where TCredentials : TokenCredentials
    {
        private readonly IProtocolLauncher _launcher;
        private readonly Activity _context;
        private ProgressDialog _progressDialog;

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
            _progressDialog = ProgressDialog.Show(
                _context,
                StringResources.ProgressDialogTitle,
                StringResources.WebProgressDialogText,
                true);

            var manager = new CustomTabsActivityManager(_context);

            manager.CustomTabsServiceConnected += (name, client) =>
            {
                var session = client.NewSession((@event, extras) =>
                {
                    if (@event == CustomTabsCallbackEvents.TAB_HIDDEN)
                    {
                    }
                    if (@event == CustomTabsCallbackEvents.TAB_SHOWN)
                    {
                        Framework.Platform.Current.RunOnMainThread(() => 
                            _progressDialog.Hide());
                    }

                });
                _launcher.ProtocolLaunch = (uri) =>
                {
                    callbackHandler(uri, manager);
                };

                var intent = new CustomTabsIntent.Builder(session).Build();

                manager.LaunchUrl(authorizationUri.ToString(), intent);
            };

            if (!manager.BindService())
            {
                throw new Exception();
            }
        }

        protected override void CleanupView(
            CustomTabsActivityManager view)
        {}

        private static class CustomTabsCallbackEvents
        {
            //Sent when the tab has started loading a page.
            public const int NAVIGATION_STARTED = 1;

            //Sent when the tab has finished loading a page.
            public const int NAVIGATION_FINISHED = 2;

            //Sent when the tab couldn't finish loading due to a failure.
            public const int NAVIGATION_FAILED = 3;

            //Sent when loading was aborted by a user action before it finishes like clicking on a link
            //or refreshing the page.
            public const int NAVIGATION_ABORTED = 4;

            //Sent when the tab becomes visible.
            public const int TAB_SHOWN = 5;

            //Sent when the tab becomes hidden.
            public const int TAB_HIDDEN = 6;
        }
    }
}