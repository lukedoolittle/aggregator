using System;
using Windows.ApplicationModel.Core;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Material.Contracts;

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

        public Action<Uri> ProtocolLaunch { get; set; }

        public void Protocol(Uri uri)
        {
            ProtocolLaunch?.Invoke(uri);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Frame Context => Window.Current.Content as Frame;

        public Action<Action> RunOnMainThread { get; } = new Action<Action>(action =>
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () => { action(); }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool IsOnline
        {
            get
            {
                var connection = NetworkInformation.GetInternetConnectionProfile();
                return connection != null &&
                       connection.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
        }

        public void Launch(Uri uri)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Windows.System.Launcher.LaunchUriAsync(uri);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool CanProvideSecureBrowsing => false;
    }
}
