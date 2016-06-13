using Aggregator.Framework.Contracts;
using Aggregator.View.BluetoothAuthorization;
using UIKit;

namespace Aggregator.Task
{
    public class BluetoothAuthorizerFactory : IBluetoothAuthorizerFactory
    {
        private readonly IBluetoothManager _manager;

        public BluetoothAuthorizerFactory(IBluetoothManager manager)
        {
            _manager = manager;
        }

        public IBluetoothAuthorizer GetAuthorizer(object context)
        {
            return new BluetoothAuthorizer(_manager, context as UIViewController);
        }
    }
}
