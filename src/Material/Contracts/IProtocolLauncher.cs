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

    public delegate void ProtocolLaunchEventHandler(
        object sender,
        ProtocolLaunchEventArgs e);

    public interface IProtocolLauncher
    {
        event ProtocolLaunchEventHandler ProtocolLaunch;
    }
}
