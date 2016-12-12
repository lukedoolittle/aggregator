using System;
using Android.App;
using Android.Content;
using Material.Contracts;
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

    //public class SecureBrowser : IBrowser
    //{
    //    public void Launch(Uri uri)
    //    {
    //        var hostedManager = new HostedActivityManager(this);

    //        var uiBuilder = new HostedUIBuilder();
    //        // Add any optional customizations here...

    //        hostedManager.LoadUrl(uri.ToString(), uiBuilder);
    //    }
    //}
}