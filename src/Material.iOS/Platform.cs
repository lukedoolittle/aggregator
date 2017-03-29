using System;
using System.Net;
using SystemConfiguration;
using CoreFoundation;
using Material.Bluetooth;
using Material.Contracts;
using Material.View.WebAuthorization;
using Material.Workflow;
using Robotics.Mobile.Core.Bluetooth.LE;
using UIKit;

namespace Material.Framework
{
    /// <summary>
    /// Platform specific functions and information for iOS
    /// </summary>
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void Initialize()
        {
            if (QuantfabricConfiguration.WebAuthorizationUIFactory == null)
            {
                QuantfabricConfiguration.WebAuthorizationUIFactory = 
                    new OAuthAuthorizerUIFactory();
            }

            if (QuantfabricConfiguration.WebAuthenticationUISelector == null)
            {
                QuantfabricConfiguration.WebAuthenticationUISelector = 
                    new MobileAuthorizationUISelector(
                        UIDevice.CurrentDevice.CheckSystemVersion(9, 0));
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

        public IAdapter BluetoothAdapter { get; } = Adapter.Current;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
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
            UIApplication.SharedApplication.InvokeOnMainThread;


        public void Launch(Uri uri)
        {
            UIApplication.SharedApplication.OpenUrl(uri.ToNSUrl());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool IsOnline => Reachability.IsReachable();
    }


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
}