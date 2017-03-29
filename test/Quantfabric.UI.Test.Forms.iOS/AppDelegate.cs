using Foundation;
using Material.Bluetooth;
using Material.Workflow;
using UIKit;

namespace Quantfabric.UI.Test.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Material.Framework.Platform.Current.Initialize();

            global::Xamarin.Forms.Forms.Init();

            Xamarin.Forms.DependencyService.Register<OAuthAuthorizerUIFactory>();
            Xamarin.Forms.DependencyService.Register<BluetoothAuthorizerUIFactory>();

            LoadApplication(new App());

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
