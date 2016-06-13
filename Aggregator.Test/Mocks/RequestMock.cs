using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;

namespace Aggregator.Test.Mocks
{
    [Aggregator.Framework.Metadata.ClientType(typeof(ClientMock))]
    [Aggregator.Framework.Metadata.ServiceType(typeof(ServiceMock))]
    public class RequestMock : Request
    {
        public static PollingInterval Interval { get; set; }
        public static string Payload { get; set; }
        public override string ResponseFilterKey { get; }
        public override string PayloadProperty => Payload;
        public override PollingInterval PollingInterval => Interval;
        public override TimestampOptions ResponseTimestamp { get; }
    }
}
