using System.Threading.Tasks;
using Material.Infrastructure.Responses;

namespace Material.Contracts
{
    public interface IGPSAdapter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Task<GPSResponse> GetPositionAsync();
    }
}
