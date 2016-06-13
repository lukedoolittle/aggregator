using Aggregator.Configuration;
using Aggregator.Test.Helpers.Fixtures;
using Xunit;
#if __MOBILE__
using Couchbase.Lite;
#endif
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

            var database = Manager.SharedInstance.GetDatabase(bucketName);

            var view = database.GetView(viewName);
            
            Assert.NotNull(view.Map);
        }
#endif
    }
}
