using System;
using System.Diagnostics;

namespace Material.Framework
{
    public partial class Platform
    {
        public static Action<Action> RunOnMainThread { get; } = null;

        public static Action<Uri> LaunchBrowser =>
            uri => Process.Start(uri.ToString());

        public static bool IsOnline => true;
    }
}
