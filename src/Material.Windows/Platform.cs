using System;
using System.Diagnostics;
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

        public static Action<Action> RunOnMainThread { get; } = null;

        public void Launch(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            Process.Start(uri.ToString());
        }

        public static bool IsOnline => true;
    }
}
