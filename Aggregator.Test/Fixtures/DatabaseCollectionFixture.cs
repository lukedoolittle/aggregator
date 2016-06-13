using System;

using Aggregator.Configuration;
using Xunit;
#if !__MOBILE__
using Couchbase.Management;
#endif
#if __MOBILE__
using Couchbase.Lite;
#endif

namespace Aggregator.Test.Helpers.Fixtures
{
    public class DatabaseCollectionFixture : IDisposable
    {
        public DatabaseCollectionFixture()
        {
        }

        public void Dispose()
        {
#if __MOBILE__
            var bucketName = ApplicationSettings.CouchbaseBucket;
            var viewName = ApplicationSettings.CouchbaseView;

            var database = Manager.SharedInstance.GetDatabase(bucketName);

            var view = database.GetView(viewName);
            view.Delete();
#else
            var configurationSection = ApplicationSettings.CouchbaseConfigSection;
            var bucketName = ApplicationSettings.CouchbaseBucket;
            var designDocumentName = ApplicationSettings.CouchbaseDesignDoc;

            var cluster = new CouchbaseCluster(configurationSection);

            cluster.DeleteDesignDocument(
                bucketName,
                designDocumentName);
#endif
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseCollectionFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
