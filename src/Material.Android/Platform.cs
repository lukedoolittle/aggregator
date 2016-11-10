using System;
using Android.App;
using Android.Content;

namespace Material.Framework
{
    public partial class Platform
    {
        public Robotics.Mobile.Core.Bluetooth.LE.IAdapter BluetoothAdapter { get; } = new Robotics.Mobile.Core.Bluetooth.LE.Adapter();

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
    }
}