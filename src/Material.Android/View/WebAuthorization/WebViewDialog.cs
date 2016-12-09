using System;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace Material.View.WebAuthorization
{
    public sealed class WebViewDialog : Dialog
    {
        private WebView _webView => FindViewById<WebView>(Resource.Id.dialogWebView);
        private ImageView _closeButton => FindViewById<ImageView>(Resource.Id.closeView);
        private readonly MyWebViewClient _webViewClient = new MyWebViewClient();
        private ProgressDialog _progressDialog;

        public WebView WebView => _webView;

        public WebViewDialog(
            Activity context,
            Action cancelledAction) : 
                base(context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.WebViewDialog);

            _closeButton.Click += (sender, args) =>
            {
                Hide();
                cancelledAction?.Invoke();
            };

            _webView.Settings.JavaScriptEnabled = true;
            _webView.Settings.BuiltInZoomControls = true;
            _webView.Settings.SetSupportZoom(true);

            _webView.SetOnTouchListener(new MyTouchListener());
            _webView.SetWebViewClient(_webViewClient);

            //Close button overlap of the webview is half of its width
            var layoutParameters = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.MatchParent,
                FrameLayout.LayoutParams.MatchParent);
            var borderWidth = _closeButton.Drawable.IntrinsicWidth / 2 + 1;
            layoutParameters.SetMargins(
                borderWidth,
                borderWidth,
                borderWidth,
                borderWidth);
            _webView.LayoutParameters = layoutParameters;
        }

        public void Show(
            Uri pageUrl, 
            Func<Uri, WebViewDialog, bool> pageStartedHandler)
        {
            if (pageUrl == null) throw new ArgumentNullException(nameof(pageUrl));
            if (pageStartedHandler == null) throw new ArgumentNullException(nameof(pageStartedHandler));

            _progressDialog = ProgressDialog.Show(
                Context,
                StringResources.ProgressDialogTitle,
                StringResources.WebProgressDialogText,
                true);

            _webViewClient.ShouldStartLoad = uri => 
                pageStartedHandler(new Uri(uri), this);

            _webViewClient.PageFinishedLoading +=
                WebViewClientOnPageFinishedLoading;

            WebView.LoadUrl(pageUrl.ToString());
        }

        private void WebViewClientOnPageFinishedLoading(
            object sender,
            EventArgs eventArgs)
        {
            _webViewClient.PageFinishedLoading -=
                WebViewClientOnPageFinishedLoading;
            _progressDialog?.Hide();
            if (!_webViewClient.StoppedLoading)
            {
                Show();
            }
        }

        private class MyWebViewClient : WebViewClient
        {
            public event EventHandler PageFinishedLoading;
            public Func<string, bool> ShouldStartLoad;
            public bool StoppedLoading { get; private set; }

            public override bool ShouldOverrideUrlLoading(
                WebView view,
                string url)
            {
                StoppedLoading = ShouldStartLoad != null && ShouldStartLoad(url);
                return StoppedLoading;
            }

            public override void OnPageFinished(
                WebView view, 
                string url)
            {
                base.OnPageFinished(view, url);

                OnPageFinishedLoading();
            }

            private void OnPageFinishedLoading()
            {
                PageFinishedLoading?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Custom OnTouchListener allows the virtual keyboard to appear when a textbox
        /// gains focus in a webview within a dialog control
        /// </summary>
        private class MyTouchListener : 
            Java.Lang.Object, 
            Android.Views.View.IOnTouchListener
        {
            public bool OnTouch(
                Android.Views.View sender, 
                MotionEvent e)
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                    case MotionEventActions.Up:
                        Android.Views.View v = sender;
                        if (!v.HasFocus)
                        {
                            v.RequestFocus();
                        }
                        break;
                }
                return false;
            }
        }
    }
}