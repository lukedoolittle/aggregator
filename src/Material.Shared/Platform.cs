using System;
#if __ANDROID__
using Plugin.CurrentActivity;
using Android.App;
using Android.Net;
using Robotics.Mobile.Core.Bluetooth.LE;
#elif __IOS__
using UIKit;
using System.Net;
using SystemConfiguration;
using CoreFoundation;
using Robotics.Mobile.Core.Bluetooth.LE;
#endif

namespace Material.Framework
{
    public static class Platform
    {
#if __ANDROID__
        public static IAdapter BluetoothAdapter { get; } = new Adapter();

        public static Activity Context => CrossCurrentActivity.Current.Activity;

        public static Action<Action> RunOnMainThread { get; } =
            action =>
            {
                Application.SynchronizationContext.Post(
                    state => action(),
                    null);
            };

        public static bool IsOnline
        {
            get
            {
                var connectivityManager = (ConnectivityManager)
                    (Application.Context.GetSystemService(
                        Android.Content.Context.ConnectivityService));

                var activeConnection = connectivityManager.ActiveNetworkInfo;

                return ((activeConnection != null) && activeConnection.IsConnected);
            }
        }

#elif __IOS__
        public static IAdapter BluetoothAdapter => Adapter.Current;

        public static UIViewController Context
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

        public static Action<Action> RunOnMainThread { get; } = 
            UIKit.UIApplication.SharedApplication.InvokeOnMainThread;

        public static bool IsOnline => Reachability.IsReachable();


#else
        public static Action<Action> RunOnMainThread { get; } = action => { };

        public static bool IsOnline => true;
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
