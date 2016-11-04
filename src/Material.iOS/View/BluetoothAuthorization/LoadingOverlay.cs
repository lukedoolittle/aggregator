using System;
using CoreGraphics;
using UIKit;

namespace Material.View.BluetoothAuthorization
{
    //adapted from https://developer.xamarin.com/recipes/ios/standard_controls/popovers/display_a_loading_message/
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class LoadingOverlay : UIView
    {
        private readonly string _overlayText;
        private UIActivityIndicatorView _activitySpinner;
        private UILabel _loadingLabel;

        public LoadingOverlay(
            CGRect frame, 
            string overlayText) : 
                base(frame)
        {
            _overlayText = overlayText;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public void Show()
        {
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;
            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // derive the center x and y
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            _activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            _activitySpinner.Frame = new CGRect(
                centerX - (_activitySpinner.Frame.Width / 2),
                centerY - _activitySpinner.Frame.Height - 20,
                _activitySpinner.Frame.Width,
                _activitySpinner.Frame.Height);
            _activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(_activitySpinner);
            _activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            _loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ))
            {
                BackgroundColor = UIColor.Clear,
                TextColor = UIColor.White,
                Text = _overlayText,
                TextAlignment = UITextAlignment.Center,
                AutoresizingMask = UIViewAutoresizing.All
            };
            AddSubview(_loadingLabel);
        }

        public void Hide()
        {
            Animate(
                0.5,
                () => { Alpha = 0; },
                RemoveFromSuperview
            );
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                // free managed resources
                if (_activitySpinner != null)
                {
                    _activitySpinner.Dispose();
                }
                if (_loadingLabel != null)
                {
                    _loadingLabel.Dispose();
                }
            }
        }
    }
}
