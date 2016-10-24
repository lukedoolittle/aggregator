using System.Threading.Tasks;
using Material.Infrastructure.Requests;

namespace Material.Contracts
{
    public interface IGPSAdapter
    {
        Task<GPSResponse> GetPositionAsync();
    }
}
