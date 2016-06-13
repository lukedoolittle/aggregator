using Android.App;
using BatmansBelt;
using Aggregator.Framework;

namespace Aggregator.Configuration
{
    public class PlatformStartupTask : IStartupTask
    {
        public void Execute()
        {
            Platform.RunOnMainThread = action =>
            {
                Application.SynchronizationContext.Post(
                    state => action(),
                    null);
            };
        }
    }
}