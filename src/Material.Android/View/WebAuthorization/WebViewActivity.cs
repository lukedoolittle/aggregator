using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace Aggregator.View.WebAuthorization
{
    [Activity(Label = "WebViewActivity")]
    public class WebViewActivity : Activity
    {
        internal static readonly ActivityStateRepository<TaskCompletionSource<WebView>> StateRepo =
            new ActivityStateRepository<TaskCompletionSource<WebView>>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var webView = new WebView(this);

            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.SetSupportZoom(true);
            webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;        
            webView.ScrollbarFadingEnabled = false;

            SetContentView(webView);

            //TODO: remove magic strings
            var stateKey = Intent.GetStringExtra("Authorizer");
            var authorizer = StateRepo.Remove(stateKey);
            authorizer.SetResult(webView);
        }
    }
}