using System;
using Aggregator.Framework.Contracts;
using Aggregator.Task;
using Aggregator.Task.Factories;
#if __MOBILE__
using Aggregator.Infrastructure.ComponentManagers;
using Robotics.Mobile.Core.Bluetooth.LE;
using Plugin.Geolocator;
#endif

namespace Aggregator.Test.Fixtures
{
    public class ClientFactoryFixture : IDisposable
    {
        public IClientFactory Factory { get; }

        public ClientFactoryFixture()
        {
            Factory = TestHelpers.CreateClientFactory();
        }

        public void Dispose()
        {
        }
    }
}
