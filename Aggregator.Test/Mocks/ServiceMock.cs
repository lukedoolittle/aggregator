using Aggregator.Domain.Write;

namespace Aggregator.Test.Mocks
{
    [Aggregator.Framework.Metadata.CredentialType(typeof(CredentialMock))]
    public class ServiceMock : Service
    {
    }
}
