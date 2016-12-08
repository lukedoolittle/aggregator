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
        public WebView WebView { get; }
        private readonly MyWebViewClient _webViewClient;
        private ProgressDialog _progressDialog;

        public WebViewDialog(
            int dialogLayout, 
            int webviewResource,
            int closeResource,
            Activity context,
            Action cancelledAction) : 
                base(context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(dialogLayout);

            var imageView = FindViewById<ImageView>(closeResource);
            imageView.Click += (sender, args) =>
            {
                Hide();
                cancelledAction?.Invoke();
            };

            WebView = FindViewById<WebView>(webviewResource);
            _webViewClient = new MyWebViewClient();

            InitializeWebViewComponent(
                WebView, 
                _webViewClient, 
                imageView);
        }

        private static void InitializeWebViewComponent(
            WebView webView,
            WebViewClient webViewClient,
            ImageView closeButton)
        {
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.SetSupportZoom(true);

            webView.SetOnTouchListener(new MyTouchListener());

            webView.SetWebViewClient(webViewClient);

            //Close button overlap of the webview is half of its width
            var layoutParameters = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.MatchParent,
                FrameLayout.LayoutParams.MatchParent);
            var borderWidth = closeButton.Drawable.IntrinsicWidth / 2 + 1;
            layoutParameters.SetMargins(
                borderWidth,
                borderWidth,
                borderWidth,
                borderWidth);
            webView.LayoutParameters = layoutParameters;
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

            _webViewClient.PageStartedLoading += (sender, uri) =>
            {
                pageStartedHandler(uri, this);
            };

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
            Show();
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

        private class MyWebViewClient : WebViewClient
        {
            public event EventHandler PageFinishedLoading;
            public event EventHandler<Uri> PageStartedLoading;
             
            public override bool ShouldOverrideUrlLoading(
                WebView view, 
                string url)
            {
                view.LoadUrl(url);

                return true;
            }

            public override void OnLoadResource(
                WebView view, 
                string url)
            {
                base.OnLoadResource(view, url);

                if (view.Progress == 100)
                {
                    OnPageFinishedLoading();
                }
            }

            public override void OnPageStarted(
                WebView view,
                string url,
                Bitmap favicon)
            {
                OnPageStartedLoading(url);
            }

            private void OnPageStartedLoading(string uri)
            {
                PageStartedLoading?.Invoke(this, new Uri(uri));
            }

            private void OnPageFinishedLoading()
            {
                PageFinishedLoading?.Invoke(this, new EventArgs());
            }
        }
    }
}