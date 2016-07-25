using Material.Infrastructure;

namespace Material.Contracts
{
    public interface ITimeseries
    {
        TimestampOptions ResponseTimestamp { get; }
    }
}
