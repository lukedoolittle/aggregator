using Material.Infrastructure;

namespace Material.Contracts
{
    public interface ITimeSeries
    {
        TimestampOptions ResponseTimestamp { get; }
    }
}
