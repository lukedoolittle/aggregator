using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace Aggregator.View.WebAuthorization
{
    public partial class WebViewController : UIViewController
    {
        private readonly CGRect _bounds;
        private readonly TaskCompletionSource<UIWebView> _taskCompletionSource; 

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

            var webView = new UIWebView(_bounds);
            View.AddSubview(webView);

            _taskCompletionSource.SetResult(webView);
        }
    }
}