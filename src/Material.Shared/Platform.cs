using System;
using Material.Contracts;

namespace Material.Framework
{
    public sealed partial class Platform : IBrowser, IProtocolLauncher
    {
        private static volatile Platform _instance;
        private static readonly object _syncLock = new object();

        private Platform() {}

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

        public void Launch(Uri uri)
        {
            LaunchBrowser(uri);
        }

        public event EventHandler<ProtocolLaunchEventArgs> ProtocolLaunch;

        public void Protocol(Uri uri)
        {
            ProtocolLaunch?.Invoke(
                this,
                new ProtocolLaunchEventArgs(uri));
        }
    }
}
