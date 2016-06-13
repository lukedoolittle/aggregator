using System;
using Newtonsoft.Json.Linq;

namespace Aggregator.Domain.Write
{
    public class Token<TService> : Token
        where TService : Service
    {
        public Token(
            Guid id,
            JObject values) :
            base(
                id,
                typeof (TService).Name,
                values)
        {
        }
    }

    public class Token
    {
        public JObject Values { get; }
        public string ServiceName { get; }
        public Guid Id { get; }

        public Token(
            Guid id, 
            string serviceName, 
            JObject values)
        {
            Id = id;
            ServiceName = serviceName;
            Values = values;
        }
    }
}
