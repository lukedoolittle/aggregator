using System;
using Foundation;
using UIKit;

namespace $rootnamespace$
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			//Necessary for creating default OAuth objects
            Material.Framework.Platform.Current.Initialize();

			return base.FinishedLaunching(app, options);
        }

	    public override bool OpenUrl(
            UIApplication app,
            NSUrl url, 
            NSDictionary options)
	    {
            //necessary for custom uri scheme OAuth callbacks to function
            Material.Framework.Platform.Current.Protocol(url);

	        return true;
	    }
    }
}


