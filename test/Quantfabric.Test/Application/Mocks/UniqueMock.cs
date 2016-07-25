using System;
using SimpleCQRS.Domain;

namespace Aggregator.Test.Helpers.Mocks
{

    public class UniqueMock : Entity
    {
        public UniqueMock(Guid id)
        {
            Id = id;
        }
    }
}
