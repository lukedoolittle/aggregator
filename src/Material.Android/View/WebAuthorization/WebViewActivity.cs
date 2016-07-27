using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace Material.View.WebAuthorization
{
    [Activity(Label = "WebViewActivity")]
    public class WebViewActivity : Activity
    {
        internal const string Authorizer = "Authorizer";

        internal static readonly ActivityStateRepository<TaskCompletionSource<WebViewActivity>> StateRepo =
            new ActivityStateRepository<TaskCompletionSource<WebViewActivity>>();

        public WebView View { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            View = new WebView(this);

            View.Settings.JavaScriptEnabled = true;
            View.Settings.BuiltInZoomControls = true;
            View.Settings.SetSupportZoom(true);
            View.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
            View.ScrollbarFadingEnabled = false;

            SetContentView(View);

            var stateKey = Intent.GetStringExtra(Authorizer);
            var authorizer = StateRepo.Remove(stateKey);
            authorizer.SetResult(this);
        }
    }
}