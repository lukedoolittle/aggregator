using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Material.Bluetooth;
using Material.Contracts;
using Material.OAuth;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace Material.Framework
{
    public class Platform : IBrowser, IProtocolLauncher
    {
        private static volatile Platform _instance;
        private static readonly object _syncLock = new object();

        private Platform() { }

        public static Platform Current
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Platform();
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            if (QuantfabricConfiguration.WebAuthorizationUIFactory == null)
            {
                QuantfabricConfiguration.WebAuthorizationUIFactory = new OAuthAuthorizerUIFactory();
            }

            if (QuantfabricConfiguration.WebAuthenticationUISelector == null)
            {
                var canProvideSecureBrowsing = false;

                try
                {
                    var packageInfo = Android.App.Application.Context.PackageManager
                        .GetApplicationInfo("com.android.chrome", 0);
                    canProvideSecureBrowsing = true;
                }
                catch (PackageManager.NameNotFoundException)
                {
                    canProvideSecureBrowsing = false;
                }

                QuantfabricConfiguration.WebAuthenticationUISelector = new MobileAuthorizationUISelector(
                    canProvideSecureBrowsing);
            }

            if (QuantfabricConfiguration.BluetoothAuthorizationUIFactory == null)
            {
                QuantfabricConfiguration.BluetoothAuthorizationUIFactory =
                    new BluetoothAuthorizerUIFactory();
            }
        }

        public Action<Uri> ProtocolLaunch { get; set; }

        public void Protocol(Uri uri)
        {
            ProtocolLaunch?.Invoke(uri);
        }

        public IAdapter BluetoothAdapter { get; } = new Adapter();

        public Activity Context { get; set; }

        public Action<Action> RunOnMainThread { get; } =
            action =>
            {
                Application.SynchronizationContext.Post(
                    state => action(),
                    null);
            };

        public bool IsOnline
        {
            get
            {
                var connectivityManager = (Android.Net.ConnectivityManager)
                    (Application.Context.GetSystemService(
                        Android.Content.Context.ConnectivityService));

                var activeConnection = connectivityManager.ActiveNetworkInfo;

                return ((activeConnection != null) && activeConnection.IsConnected);
            }
        }

        public void Launch(Uri uri)
        {
            var neturi = Android.Net.Uri.Parse(uri.ToString());
            var intent = new Intent(Intent.ActionView, neturi);
            Context.StartActivity(intent);
        }
    }
}