using Aggregator.Configuration;
using Aggregator.Test.Helpers.Fixtures;
using Xunit;

namespace Aggregator.Test.Integration.Configuration
{
    [CollectionDefinition("Database collection")]
    public class CouchbaseConfigurationTaskTests : 
        IClassFixture<CouchbaseConfigurationFixture>
    {
#if __MOBILE__
        [Fact]
        public void RunCouchbaseConfigurationAndViewShouldBePresentInDatabase()
        {
            var bucketName = ApplicationSettings.CouchbaseBucket;
            var viewName = ApplicationSettings.CouchbaseView;

            var database = Couchbase.Lite.Manager.SharedInstance.GetDatabase(bucketName);

            var view = database.GetView(viewName);
            
            Assert.NotNull(view.Map);
        }
#endif
    }
}
