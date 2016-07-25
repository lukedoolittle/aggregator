using Foundation;
using UIKit;

namespace Aggregator.UI.Test.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : LoggingAggregatorAppDelegate
    {
		public override UIWindow Window
		{
			get;
			set;
		}
    }
}


