using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Commands;
using Foundations.Cryptography;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Requests
{
    using System.Threading.Tasks;

    public abstract class SampleRequestTaskBase<TRequest> :
        ITask
        where TRequest : Request, new()
    {
        private readonly ICommandSender _sender;
        private readonly IEventPublisher _publisher;
        private readonly Guid _aggregateId;
        protected string _lastQuery;

        protected SampleRequestTaskBase(
            IEventPublisher publisher,
            ICommandSender sender,
            Guid aggregateId,
            string lastQuery)
        {
            _lastQuery = lastQuery;
            _publisher = publisher;
            _sender = sender;
            _aggregateId = aggregateId;
        }

        public async Task Execute(object parameter = null)
        {
            var samples = await GetSamples().ConfigureAwait(false);

            var enumerable = samples as Tuple<DateTimeOffset, JObject>[] ?? 
                samples.ToArray();

            foreach (var sample in enumerable)
            {
                var hash = Security.CreateGuidFromData(sample.Item2.ToString());
                var @event = new SampleAdded<TRequest>(
                    sample.Item2,
                    sample.Item1,
                    hash)
                {
                    AggregateId = _aggregateId,
                    Version = 0
                };
                await _publisher.Publish(@event)
                        .ConfigureAwait(false);
            }

            if (enumerable.Any())
            {
                await _sender.Send(new ChangeFilterCommand<TRequest>(
                        _aggregateId, 
                        0, 
                        enumerable))
                    .ConfigureAwait(false);
            }
        }

        public void Handle(FilterChanged<TRequest> @event)
        {
            _lastQuery = @event.NewFilter;
        }

        protected abstract Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetSamples();
    }
}
