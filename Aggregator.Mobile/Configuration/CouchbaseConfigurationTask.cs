using BatmansBelt;
using BatmansBelt.Metadata;
using Couchbase.Lite;

namespace Aggregator.Configuration.Startup
{
    [Dependency(typeof(SerializationStartupTask))]
    public class CouchbaseConfigurationTask : IStartupTask
    {
        private readonly string _bucketName;
        private readonly string _viewName;

        public CouchbaseConfigurationTask()
        {
            _bucketName = ApplicationSettings.CouchbaseBucket;
            _viewName = ApplicationSettings.CouchbaseView;
        }
        public void Execute()
        {
            var database = Manager.SharedInstance.GetDatabase(_bucketName);

            var view = database.GetView(_viewName);

            view.SetMap((doc, emit) =>
            {
				object type;
				object aggregateId;
				doc.TryGetValue("Type", out type);
				doc.TryGetValue("AggregateId", out aggregateId);
				if (type != null && aggregateId != null)
				{
					emit(aggregateId.ToString() + type.ToString(), doc);
				}
            }, "1.2");
				
            var query = view.CreateQuery();
        }
    }
}
