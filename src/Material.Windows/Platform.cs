using System;
using System.Diagnostics;
using Material.Contracts;
using Material.OAuth;

namespace Material.Framework
{
    /// <summary>
    /// Platform specific functions and information for Windows
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
                QuantfabricConfiguration.WebAuthorizationUIFactory = new OAuthAuthorizerUIFactory();
            }

            if (QuantfabricConfiguration.WebAuthenticationUISelector == null)
            {
                QuantfabricConfiguration.WebAuthenticationUISelector = new WindowsAuthorizationUISelector();
            }
        }

        public Action<Uri> ProtocolLaunch { get; set; }

        public void Protocol(Uri uri)
        {
            ProtocolLaunch?.Invoke(uri);
        }

        public Action<Action> RunOnMainThread { get; } = null;

        public void Launch(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            Process.Start(uri.ToString());
        }
    }
}
