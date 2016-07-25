using System;
using System.Collections.Generic;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Helpers
{
    public class TrackingDatabaseDecorator<TEntity> : IDatabase<TEntity>
        where TEntity : Entity
    {
        private readonly IDatabase<TEntity> _database;
        private readonly Action<TEntity> _actionWhenAdding;

        public TrackingDatabaseDecorator(
            IDatabase<TEntity> database,
            Action<TEntity> actionWhenAdding)
        {
            _database = database;
            _actionWhenAdding = actionWhenAdding;
        } 

        public TEntity Get(string id)
        {
            return _database.Get(id);
        }

        public IEnumerable<TEntity> GetAll(string aggregateId)
        {
            return _database.GetAll(aggregateId);
        }

        public void Update(TEntity item)
        {
            _database.Update(item);
        }

        public void Put(TEntity item)
        {
            _actionWhenAdding(item);
            _database.Put(item);
        }

        public void Delete(string id)
        {
            _database.Delete(id);
        }
    }
}
