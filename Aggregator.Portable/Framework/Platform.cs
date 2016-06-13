using System;
using Plugin.Connectivity;

namespace Aggregator.Framework
{
    public static class Platform
    {
        public static Action<Action> RunOnMainThread { get; set; }
        public static bool IsOnline => CrossConnectivity.Current.IsConnected;
    }
}
