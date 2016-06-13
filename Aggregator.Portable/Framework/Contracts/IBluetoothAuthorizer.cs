using System;
using System.Threading.Tasks;

namespace Aggregator.Framework.Contracts
{
    public interface IBluetoothAuthorizer
    {
        Task<Guid> GetDeviceUuid();
    }
}
