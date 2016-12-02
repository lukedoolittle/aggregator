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
            //Uncomment the below lines if using Xamarin.Forms
            //global::Xamarin.Forms.Forms.Init();
            //Xamarin.Forms.DependencyService.Register<OAuthAuthorizerUIFactory>();
            //Xamarin.Forms.DependencyService.Register<BluetoothAuthorizerUIFactory>();
            //LoadApplication(new App());
            //return base.FinishedLaunching(app, options);
            
            return true;
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


