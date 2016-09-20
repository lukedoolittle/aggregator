using Material.Contracts;
using Material.Infrastructure;
using Xamarin.Forms;

namespace Material
{
    public class BluetoothApp<TResourceProvider> : BluetoothAppBase<TResourceProvider>
        where TResourceProvider : BluetoothResourceProvider, new()
    {
        public BluetoothApp() : 
            base(DependencyService.Get<IBluetoothAuthorizerUIFactory>())
        { }
    }
}
