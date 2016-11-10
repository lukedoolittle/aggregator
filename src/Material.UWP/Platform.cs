using System;
using Windows.ApplicationModel.Core;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Material.Framework
{
    public partial class Platform
    {
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Action<Uri> LaunchBrowser =>
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            uri => Windows.System.Launcher.LaunchUriAsync(uri);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }
}
