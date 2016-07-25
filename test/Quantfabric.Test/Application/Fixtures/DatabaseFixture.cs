using System;
using System.Collections.Concurrent;
using Aggregator.Configuration;
using Aggregator.Infrastructure.Repositories;
using Aggregator.Test.Helpers.Fixtures;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;
#if !__MOBILE__
using Couchbase;
#endif
#if __MOBILE__
using Couchbase.Lite;
#endif

namespace Aggregator.Test.Helpers
{
    public class DatabaseFixture : CouchbaseConfigurationFixture
    {
#if !__MOBILE__
        private readonly CouchbaseClient _client;
#endif
        private readonly ConcurrentBag<Guid> _addedEntities;
         
		public DatabaseFixture()
			: base()
		{
			_addedEntities = new ConcurrentBag<Guid>();
#if !__MOBILE__
			_client = new CouchbaseClient();
#endif
		}
			
        public IDatabase<T> GetDatabase<T>()
            where T : Entity
        {
            return new TrackingDatabaseDecorator<T>(
                CreateDatabase<T>(),
                itemId => _addedEntities.Add(itemId.Id));
        }

        private CouchbaseDatabase<T> CreateDatabase<T>()
            where T : Entity
        {
            var couchbaseViewConfiguration = new CouchbaseView(
                ApplicationSettings.CouchbaseDesignDoc,
                ApplicationSettings.CouchbaseView);
#if !__MOBILE__
            return new CouchbaseDatabase<T>(
                _client,
                couchbaseViewConfiguration);
#else
            return new CouchbaseDatabase<T>(
                Manager.SharedInstance.GetDatabase("default"), 
                couchbaseViewConfiguration);
#endif
        }
			
        public override void Dispose()
        {
            base.Dispose();

            foreach (var itemId in _addedEntities)
            {
#if !__MOBILE__
                _client.ExecuteRemove(itemId.ToString());
#else
				var document = Manager.SharedInstance.GetDatabase("default").GetExistingDocument(itemId.ToString());
				document.Delete();
				//if (!document.Deleted)
#endif
            }
#if !__MOBILE__
            _client?.Dispose();
#endif
        }
    }
}
