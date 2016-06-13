using System;

namespace Aggregator.Test.Helpers.Fixtures
{
    public class CouchbaseConfigurationFixture : IDisposable
    {
        public CouchbaseConfigurationFixture()
        {
            TestHelpers.InitializeCouchbaseDatabase();
        }

        public virtual void Dispose()
        {
        }
    }
}
