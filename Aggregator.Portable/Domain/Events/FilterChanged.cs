using Aggregator.Domain.Write;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class FilterChanged<TRequest> : Event
        where TRequest : Request
    {
        public string NewFilter { get; private set; }

        public FilterChanged(string newFilter)
        {
            NewFilter = newFilter;
        }
    }
}
