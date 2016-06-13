using System;
using System.Collections.Generic;
using Aggregator.Test.Helpers.Mocks;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Mocks
{
    class RepositoryMock<TAggregateRoot> : 
        MockBase<RepositoryMock<TAggregateRoot>>, 
        IRepository<TAggregateRoot> where 
        TAggregateRoot : AggregateRoot, new()
    {
        private readonly IDictionary<Guid, TAggregateRoot> _aggregateRoots;

        public RepositoryMock()
        {
            _aggregateRoots = new Dictionary<Guid, TAggregateRoot>();
        } 

        public void Save(
            TAggregateRoot aggregate, 
            int expectedVersion)
        {
            if (_aggregateRoots.ContainsKey(aggregate.Id))
            {
                _aggregateRoots[aggregate.Id] = aggregate;
            }
            else
            {
                _aggregateRoots.Add(aggregate.Id, aggregate);
            }
        }

        public TAggregateRoot GetById(Guid aggregateId)
        {
            return _aggregateRoots[aggregateId];
        }
    }
}
