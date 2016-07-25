using Aggregator.Domain.Write;
using Material.Infrastructure;
using Material.Metadata;

namespace Aggregator.Test.Mocks
{
    [CredentialType(typeof(CredentialMock))]
    public class ResourceProviderMock : ResourceProvider
    {
    }
}
