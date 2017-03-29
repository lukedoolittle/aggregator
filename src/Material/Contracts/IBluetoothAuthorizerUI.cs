using System.Threading.Tasks;
using Material.Domain.Credentials;

namespace Material.Contracts
{
    public interface IBluetoothAuthorizerUI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Task<BluetoothCredentials> GetDeviceUuid();
    }
}
