using System.Threading.Tasks;
using Material.Infrastructure.Static;

namespace Material.Contracts
{
    public interface IGPSAdapter
    {
        Task<GPSResponse> GetPositionAsync();
    }
}
