using System;
using System.Threading.Tasks;
using Foundation;
using Material.Contracts;
using Material.Domain.Credentials;
using Material.Framework.Enums;
using Material.Workflow.Template;
using SafariServices;
using UIKit;

namespace Material.View.WebAuthorization
{
    public class SafariWebViewAuthorizerUI<TCredentials> : 
        OAuthAuthorizationUITemplateBase<TCredentials, EventedSafariViewController>
        where TCredentials : TokenCredentials
    {
        private readonly IProtocolLauncher _launcher;
        private readonly UIViewController _context;

        public SafariWebViewAuthorizerUI(
            IProtocolLauncher launcher,
            UIViewController context,
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
            Func<Uri, EventedSafariViewController, bool> callbackHandler)
        {
            var sfViewController = new EventedSafariViewController(
                authorizationUri);
            sfViewController.Canceled += (sender, args) =>
            {
                credentialsCompletion.SetCanceled();
            };

            _launcher.ProtocolLaunch = (uri) =>
            {
                callbackHandler(uri, sfViewController);
            };

            _context.PresentViewControllerAsync(
                sfViewController, 
                true);
        }

        protected override void CleanupView(
            EventedSafariViewController view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            view.DismissViewController(
                true, 
                null, 
                false);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class EventedSafariViewController : SFSafariViewController
    {
        public event EventHandler Canceled;

        public EventedSafariViewController(NSCoder coder) : 
            base(coder)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "t")]
        public EventedSafariViewController(NSObjectFlag t) : 
            base(t)
        {
        }

        public EventedSafariViewController(IntPtr handle) : 
            base(handle)
        {
        }

        public EventedSafariViewController(
            string nibName, 
            NSBundle bundle) : 
            base(nibName, bundle)
        {
        }

        public EventedSafariViewController(
            NSUrl url, 
            bool entersReaderIfAvailable) : 
                base(url, entersReaderIfAvailable)
        {
        }

        public EventedSafariViewController(NSUrl url) :
            base(url)
        {
        }

        public override void DismissViewController(
            bool animated,
            Action completionHandler)
        {
            DismissViewController(
                animated, 
                completionHandler,
                true);
        }

        public void DismissViewController(
            bool animated,
            Action completionHandler, 
            bool canceled)
        {
            base.DismissViewController(animated, completionHandler);

            if (canceled)
            {
                Canceled?.Invoke(this, new EventArgs());
            }
        }
    }
}