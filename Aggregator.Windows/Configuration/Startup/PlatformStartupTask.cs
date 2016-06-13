using BatmansBelt;
using Aggregator.Framework;

namespace Aggregator.Configuration.Startup
{
    public class PlatformStartupTask : IStartupTask
    {
        public void Execute()
        {
            //Since there isn't a unified main thread for windows applications
            //just run on the current thread. Note this should be overridden
            //in Windows applications with a GUI thread
            if (Platform.RunOnMainThread == null)
            {
                Platform.RunOnMainThread = action => { };
            }
        }
    }
}
