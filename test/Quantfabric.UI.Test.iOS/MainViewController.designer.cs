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

namespace Aggregator.UI.Test.iOS
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FacebookAuth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MioAuth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView ResultsTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TwitterAuth { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FacebookAuth != null) {
                FacebookAuth.Dispose ();
                FacebookAuth = null;
            }

            if (MioAuth != null) {
                MioAuth.Dispose ();
                MioAuth = null;
            }

            if (ResultsTextView != null) {
                ResultsTextView.Dispose ();
                ResultsTextView = null;
            }

            if (TwitterAuth != null) {
                TwitterAuth.Dispose ();
                TwitterAuth = null;
            }
        }
    }
}