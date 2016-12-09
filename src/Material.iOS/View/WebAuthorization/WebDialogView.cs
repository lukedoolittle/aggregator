using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Material.View.WebAuthorization
{
    public partial class WebDialogView : UIView
    {
        private static double BorderPaddingPercentageX = .05;
        private static double BorderPaddingPercentageY = .05;

        private UIViewController _context;
        private Action _canceled;

        public UIWebView WebView => _webView;

        public WebDialogView (IntPtr handle) : 
            base (handle)
        { }

        public static WebDialogView Create(
            UIViewController context,
            Action canceled)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var view = Runtime.GetNSObject<WebDialogView>(
                NSBundle.MainBundle.LoadNib(
                        typeof(WebDialogView).Name,
                        null,
                        null)
                    .ValueAt(0));

            view._context = context;
            view._canceled = canceled;
            var frame = context.View.Frame;
            view.Frame = new CGRect(
                frame.Width * BorderPaddingPercentageX,
                frame.Height * BorderPaddingPercentageY,
                frame.Width - 2 * BorderPaddingPercentageX * frame.Width,
                frame.Height - 2 * BorderPaddingPercentageY * frame.Height);

            return view;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public void Show(
            Uri url,
            Func<Uri, WebDialogView, bool> callbackHandler)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            _closeButton.TouchUpInside += (sender, args) =>
            {
                Hide();
                _canceled?.Invoke();
            };

            _webView.ShouldStartLoad = (view, request, type) =>
            {
                var success = callbackHandler(
                    new Uri(request.Url.ToString()),
                    this);

                return !success;
            };
            _webView.LoadFinished += WebViewOnLoadFinished;
            _webView.LoadRequest(new NSUrlRequest(url.ToNSUrl()));

            ResizeLayout();
            _activityIndicator.StartAnimating();
            _context.View.AddSubview(this);
        }

        private void WebViewOnLoadFinished(
            object sender, 
            EventArgs eventArgs)
        {
            _webView.LoadFinished -= WebViewOnLoadFinished;

            _closeButton.Hidden = false;
            _webView.Hidden = false;

            _activityIndicator.StopAnimating();
        }

        public void Hide()
        {
            RemoveFromSuperview();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ResizeLayout();
        }

        private void ResizeLayout()
        {
            var padding =
                _closeButton.CurrentImage.Size.Width / 2;

            _webView.Frame = new CGRect(
                padding,
                padding,
                Frame.Width - 2 * padding,
                Frame.Height - 2 * padding);

            var centerX = Frame.Width / 2;
            var centerY = Frame.Height / 2;

            _activityIndicator.Frame = new CGRect(
                centerX - _activityIndicator.Frame.Width / 2,
                centerY - _activityIndicator.Frame.Height / 2,
                _activityIndicator.Frame.Width,
                _activityIndicator.Frame.Height);
        }
    }
}