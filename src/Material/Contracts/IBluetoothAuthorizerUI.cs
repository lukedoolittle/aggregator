using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IBluetoothAuthorizerUI
    {
        Task<BluetoothCredentials> GetDeviceUuid();
    }
}
