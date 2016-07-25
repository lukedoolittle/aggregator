#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase.Lite;
using Aggregator.Framework.Exceptions;
using Foundations.Serialization;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Infrastructure.Repositories
{
    public class CouchbaseDatabase<TEntity> : 
        CouchbaseDatabase, 
        IDatabase<TEntity>
        where TEntity : Entity
    {
        public CouchbaseDatabase(
            Database client,
            CouchbaseView getAllView) : 
            base(client, getAllView)
        {
        }

        public void Put(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            var properties = item.AsDictionary();

            if (!KeysAreValidForInsertion(properties))
            {
                throw new CouchbaseInvalidPropertyNameException();
            }

            try
            {
                var document = _client.GetDocument(item.Id.ToString());
                document.PutProperties(properties);
            }
            catch (CouchbaseLiteException)
            {
                throw new CouchbaseDuplicateStorageKeyException();
            }
        }

        public void Update(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            var document = _client.GetExistingDocument(item.Id.ToString());

            if (document == null)
            {
                Put(item);
            }
            else
            {
                var properties = item.AsDictionary();

                if (!KeysAreValidForInsertion(properties))
                {
                    throw new CouchbaseInvalidPropertyNameException();
                }

                properties.Add("_rev", document.Properties["_rev"]);

                if (document.PutProperties(properties) == null)
                {
                    throw new CouchbaseConcurrencyException();
                }
            }
        }

        public void Delete(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            this.Delete(item.Id.ToString());
        }

        public void Delete(string id)
        {
            var doc = _client.GetExistingDocument(id);
            doc?.Delete();
        }

        public TEntity Get(string id)
        {
            var doc = _client.GetExistingDocument(id);

            return doc?.Properties?.AsEntity<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(string id)
        {
            return base.GetAll<TEntity>(id);
        }

        /// <summary>
        /// Couchbaselite will not allow keys to start with either '_' or '$' which prevents
        /// properly named private properties and objects serialized with TypeNameHandling.All
        /// from being inserted. This method determines if any keys meet that criteria
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private bool KeysAreValidForInsertion(IDictionary<string, object> properties)
        {
            return properties.Keys.All(key => !key.StartsWith("_") && !key.StartsWith("$"));
        }
    }

    public class CouchbaseDatabase : IDatabase
    {
        protected readonly Database _client;
        private readonly string _viewName;

        public CouchbaseDatabase(
            Database client,
            CouchbaseView getAllView)
        {
            _client = client;
            _viewName = getAllView.ViewName;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(string id)
            where TEntity : Entity
        {
            List<TEntity> entities = new List<TEntity>();

            var query = GetView().CreateQuery();
			var key = id + typeof(TEntity).AssemblyQualifiedName;
            query.StartKey = key;
            query.EndKey = key;
            var resultRows = query.Run();

            foreach (var row in resultRows)
            {
				var document = _client.GetExistingDocument(row.DocumentId);
				var entity = document.Properties.AsEntity<TEntity>();
				entities.Add(entity);
            }

            return entities;
        }

        private Couchbase.Lite.View GetView()
        {
            var view = _client.GetExistingView(_viewName);

			if (view?.Map == null)
            {
                throw new CouchbaseViewDoesNotExistException();
            }

            if (view.IsStale)
            {
                view.DeleteIndex();
            }

            return view;
        }
    }
}
#endif