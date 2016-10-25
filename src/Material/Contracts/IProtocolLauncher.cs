using System;

namespace Material.Contracts
{
    public class ProtocolLaunchEventArgs : EventArgs
    {
        public Uri Uri { get; set; }

        public ProtocolLaunchEventArgs(Uri uri)
        {
            Uri = uri;
        }
    }

    public interface IProtocolLauncher
    {
        event EventHandler<ProtocolLaunchEventArgs> ProtocolLaunch;
    }
}
