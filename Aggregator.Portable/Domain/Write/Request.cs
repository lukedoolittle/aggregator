using Aggregator.Framework.Enums;
using Aggregator.Framework.Exceptions;

namespace Aggregator.Domain.Write
{
    public abstract class Request
    {
        public abstract string ResponseFilterKey { get; }
        public abstract string PayloadProperty { get; }
        public virtual PollingInterval PollingInterval { get; }
        public virtual TimestampOptions ResponseTimestamp { get; }
    }

    public class TimestampOptions
    {
        public TimestampOptions(
            string timestampProperty,
            string timestampFormat,
            string timestampOffsetProperty,
            string timestampOffset)
        {
            TimestampProperty = timestampProperty;
            TimestampFormat = timestampFormat;
            TimestampOffsetProperty = timestampOffsetProperty;
            TimestampOffset = timestampOffset;
        }

        public string TimestampProperty { get; }
        public string TimestampFormat { get; }
        public string TimestampOffsetProperty { get; }
        public string TimestampOffset { get; }
    }

    public class PollingInterval
    {
        public int this[PollingIntervalEnum key]
        {
            get
            {
                switch (key)
                {
                    case PollingIntervalEnum.Fast:
                        return FastPolling;
                    case PollingIntervalEnum.Moderate:
                        return ModeratePolling;
                    case PollingIntervalEnum.Slow:
                        return SlowPolling;
                    default:
                        throw new InvalidPollingIntervalException();
                }
            }
        }
        public PollingInterval(
            int fastPolling, 
            int moderatePolling, 
            int slowPolling)
        {
            FastPolling = fastPolling;
            ModeratePolling = moderatePolling;
            SlowPolling = slowPolling;
        }

        public int FastPolling { get; }
        public int ModeratePolling { get; }
        public int SlowPolling { get; }
    }
}
