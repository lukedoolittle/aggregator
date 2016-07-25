namespace Material.Infrastructure
{
    public abstract class Request
    {
        public virtual string ResponseFilterKey { get; }
        public virtual string PayloadProperty { get; }

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
}
