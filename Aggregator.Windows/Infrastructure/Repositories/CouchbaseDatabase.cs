using System.Collections.Generic;
using System.Linq;
using BatmansBelt.Serialization;
using Couchbase;
using Enyim.Caching.Memcached;
using Aggregator.Framework.Exceptions;
using Aggregator.Framework.Serialization;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;
using static System.String;

namespace Aggregator.Infrastructure.Repositories
{
    public class CouchbaseDatabase<TEntity> : 
        CouchbaseDatabase, 
        IDatabase<TEntity>
        where TEntity : Entity
    {
        public CouchbaseDatabase(
            CouchbaseClient client,
            CouchbaseView getAllView) : 
            base(client, getAllView)
        {
        }

        public void Put(TEntity item)
        {
            var result = _client.Store(
                StoreMode.Add,
                item.Id.ToString(),
                item.AsJson());

            if (!result)
            {
                throw new CouchbaseDuplicateStorageKeyException();
            }
        }

        public void Update(TEntity item)
        {
            var result = _client.Store(
                StoreMode.Set,
                item.Id.ToString(),
                item.AsJson());

            if (!result)
            {
                throw new CouchbaseDuplicateStorageKeyException();
            }
        }

        public void Delete(TEntity item)
        {
            this.Delete(item.Id.ToString());
        }

        public void Delete(string id)
        {
            _client.Remove(id);
        }

        public TEntity Get(string id)
        {
            var json = _client.Get<string>(id);

            if (IsNullOrEmpty(json))
            {
                return default(TEntity);
            }

            var entity = json.AsEntity<TEntity>();

            return entity;
        }

        public IEnumerable<TEntity> GetAll(string id)
        {
            return base.GetAll<TEntity>(id);
        }
    }

    public class CouchbaseDatabase : IDatabase
    {
        protected readonly CouchbaseClient _client;
        private readonly string _designDocumentName;
        private readonly string _viewName;

        public CouchbaseDatabase(
            CouchbaseClient client,
            CouchbaseView getAllView)
        {
            _client = client;
            _designDocumentName = getAllView.DesignDocument;
            _viewName = getAllView.ViewName;
        }

        public IEnumerable<T> GetAll<T>(string aggregateId)
            where T : Entity
        {
            var key = aggregateId + typeof (T).AssemblyQualifiedName;

            var view = _client.GetView(
                _designDocumentName,
                _viewName)
                .StartKey(key)
                .EndKey(key)
                .Stale(StaleMode.False);

            return view
                .Select(row => row.GetItem().ToString())
                .Select(json => json.AsEntity<T>())
                .ToList();
        }
    }
}
