using System;
using Newtonsoft.Json.Linq;

namespace Aggregator.Framework.Contracts
{
    public interface ISampleRequestTaskFactory
    {
        ITask GetTask(
            Guid aggregateId,
            JObject credentials,
            Type requestType,
            string lastQuery);
    }
}
