using System;
using System.Collections.Generic;
using Aggregator.Domain.Write;
using Aggregator.Domain.Write.Samples;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Domain.Read
{
    public class ReadModelFacade
    {
        private readonly IDatabase _database;

        public ReadModelFacade(IDatabase database)
        {
            _database = database;
        }

        public IEnumerable<SensorDto> GetSensors(Guid aggregateId)
        {
            return _database.GetAll<SensorDto>(aggregateId.ToString());
        }

        public IEnumerable<SampleDto<TRequest>> GetSamples<TRequest>(Guid aggregateId)
            where TRequest : Request
        {
            return _database.GetAll<SampleDto<TRequest>>(aggregateId.ToString());
        } 
    }
}
