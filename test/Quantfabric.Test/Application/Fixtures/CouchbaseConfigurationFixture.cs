using System;
using Aggregator.Configuration.Startup;

namespace Aggregator.Test.Helpers.Fixtures
{
    public class CouchbaseConfigurationFixture : IDisposable
    {
        public CouchbaseConfigurationFixture()
        {
            new CouchbaseConfigurationTask().Execute();
        }

        public virtual void Dispose()
        {
        }
    }
}
