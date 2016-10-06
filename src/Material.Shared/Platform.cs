using System;
using Material.Contracts;
#if __ANDROID__
using Android.App;
using Robotics.Mobile.Core.Bluetooth.LE;
using Android.Content;
#elif __IOS__
using UIKit;
using Foundation;
using System.Net;
using SystemConfiguration;
using CoreFoundation;
using Robotics.Mobile.Core.Bluetooth.LE;
#endif
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.Networking.Connectivity;
#endif
#if __WINDOWS__
using System.Diagnostics;
#endif

namespace Material.Framework
{

    public class Platform : IBrowser, IProtocolLauncher
    {
        private static Platform _instance;
        private static readonly object _syncLock = new object();

        protected Platform() {}

        public static Platform Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Platform();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Launch(Uri uri)
        {
            LaunchBrowser(uri);
        }

        public event ProtocolLaunchEventHandler ProtocolLaunch;

        protected virtual void OnProtocolLaunched(Uri uri)
        {
            ProtocolLaunch?.Invoke(
                this,
                new ProtocolLaunchEventArgs(uri));
        }

        public void Protocol(Uri uri)
        {
            OnProtocolLaunched(uri);
        }

#if __ANDROID__
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

        public Action<System.Uri> LaunchBrowser
        {
            get
            {
                return uri =>
                {
                    var neturi = Android.Net.Uri.Parse(uri.ToString());
                    var intent = new Intent(Intent.ActionView, neturi);
                    Context.StartActivity(intent);
                };
            }
        }

#elif __IOS__
        public IAdapter BluetoothAdapter => Adapter.Current;

        public UIViewController Context
        {
            get
            {
                var viewController = UIApplication
                    .SharedApplication
                    .KeyWindow
                    .RootViewController;
                while (viewController.PresentedViewController != null)
                {
                    viewController = viewController.PresentedViewController;
                }

                return viewController;
            }
        }

        public Action<Action> RunOnMainThread { get; } = 
            UIKit.UIApplication.SharedApplication.InvokeOnMainThread;

        public Action<Uri> LaunchBrowser => 
            uri => UIApplication.SharedApplication.OpenUrl(
                new NSUrl(
                    uri.ToString()));

        public bool IsOnline => Reachability.IsReachable();

#elif WINDOWS_UWP
        public Frame Context => Window.Current.Content as Frame;

        public Action<Action> RunOnMainThread { get; } = new Action<Action>(action => 
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, 
                () => { action(); }));

        public bool IsOnline
        {
            get
            {
                var connection = NetworkInformation.GetInternetConnectionProfile();
                return connection != null &&
                       connection.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
        }

        public Action<Uri> LaunchBrowser => 
            uri => Windows.System.Launcher.LaunchUriAsync(uri);
#else
        public Action<Action> RunOnMainThread { get; } = action => { };

        public Action<Uri> LaunchBrowser => 
            uri => Process.Start(uri.ToString());

        public bool IsOnline => true;
#endif
    }

#if __IOS__
    //https://github.com/jamesmontemagno/ConnectivityPlugin/blob/master/src/Connectivity.Plugin.iOS/Reachability.cs
    public static class Reachability
    {
        private static NetworkReachability defaultRouteReachability;

        public static bool IsReachable()
        {
            NetworkReachabilityFlags flags;

            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            var defaultRouteAvailable = defaultRouteReachability.TryGetFlags(out flags);


            var isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            var noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0
                                        || (flags & NetworkReachabilityFlags.IsWWAN) != 0;

            var canReachWithoutConnection =
                isReachable && 
                noConnectionRequired;

            var defaultNetworkAvailable =
                defaultRouteAvailable &&
                canReachWithoutConnection;

            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
                return false;
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return true;
            else if (flags == 0)
                return false;
            return true;
        }
    }
#endif
}
