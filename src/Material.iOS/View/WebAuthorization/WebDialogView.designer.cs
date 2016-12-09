// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Material.View.WebAuthorization
{
    [Register ("WebDialogView")]
    partial class WebDialogView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView _activityIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton _closeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView _webView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_activityIndicator != null) {
                _activityIndicator.Dispose ();
                _activityIndicator = null;
            }

            if (_closeButton != null) {
                _closeButton.Dispose ();
                _closeButton = null;
            }

            if (_webView != null) {
                _webView.Dispose ();
                _webView = null;
            }
        }
    }
}