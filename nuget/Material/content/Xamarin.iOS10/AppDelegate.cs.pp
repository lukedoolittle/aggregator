using System;
using Foundation;
using UIKit;

namespace $rootnamespace$
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

	    public override bool OpenUrl(
            UIApplication app,
            NSUrl url, 
            NSDictionary options)
	    {
            var callbackUri = "";  //TODO: add OAuth callback uri

            if (url.ToString().StartsWith(callbackUri))
            {
                //necessary for custom uri scheme OAuth callbacks to function
                Material.Framework.Platform.Current.Protocol(url);
            }

	        return true;
	    }

	    public override UIWindow Window
		{
			get;
			set;
		}
    }
}


