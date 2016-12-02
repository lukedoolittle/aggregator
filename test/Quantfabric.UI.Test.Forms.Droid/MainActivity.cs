using Android.App;
using Android.Content.PM;
using Android.OS;
using Material.Bluetooth;
using Material.OAuth;

namespace Quantfabric.UI.Test.Forms.Droid
{
    [Activity(Name = "quantfabric.ui.test.MainActivity", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Xamarin.Forms.DependencyService.Register<OAuthAuthorizerUIFactory>();
            Xamarin.Forms.DependencyService.Register<BluetoothAuthorizerUIFactory>();

            LoadApplication(new App());
        }
    }
}

