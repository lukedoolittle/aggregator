using System;
using Foundation;
using UIKit;

namespace Quantfabric.UI.Test.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            return true;
        }

        public override void OnActivated(UIApplication application)
        {
        }

        public override void PerformFetch(
            UIApplication application,
            Action<UIBackgroundFetchResult> completionHandler)
        {

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override void DidEnterBackground(UIApplication application)
        {

        }
		public override UIWindow Window
		{
			get;
			set;
		}
    }
}


