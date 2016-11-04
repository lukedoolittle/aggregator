using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace Material.View.WebAuthorization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public partial class WebViewController : UIViewController
    {
        private readonly CGRect _bounds;
        private readonly TaskCompletionSource<UIWebView> _taskCompletionSource;
        private UIWebView _webView;

        public WebViewController(
            CGRect bounds,
            TaskCompletionSource<UIWebView> taskCompletionSource)
        {
            _bounds = bounds;
            _taskCompletionSource = taskCompletionSource;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "WebView";
            View.BackgroundColor = UIColor.White;

            _webView = new UIWebView(_bounds);
            View.AddSubview(_webView);

            _taskCompletionSource.SetResult(_webView);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                // free managed resources
                if (_webView != null)
                {
                    _webView.Dispose();
                }
            }
        }
    }
}