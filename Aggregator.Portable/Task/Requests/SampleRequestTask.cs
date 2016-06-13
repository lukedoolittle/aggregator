using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Exceptions;
using Aggregator.Infrastructure.Credentials;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Requests
{
    using System.Threading.Tasks;

    public class SampleRequestTask<TRequest, TCredentials, TService> : 
        SampleRequestTaskBase<TRequest>
        where TRequest : Request, new()
        where TService : Service
        where TCredentials : TokenCredentials
    {
        private readonly IClientFactory _clientFactory;
        private TCredentials _credentials;
        private readonly ITask _updateCredentialsTask;

        public SampleRequestTask(
            IClientFactory clientFactory, 
            ICommandSender sender, 
            IEventPublisher publisher,
            TCredentials credentials,
            ITask updateCredentialsTask,
            Guid aggregateId,
            string lastQuery) :
            base(
                publisher,
                sender, 
                aggregateId,
                lastQuery)
        {
            _clientFactory = clientFactory;
            _credentials = credentials;
            _updateCredentialsTask = updateCredentialsTask;
        }

        protected override Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetSamples()
        {
            Task updateTokenResult = Task.FromResult(new object());
            
            if (_credentials.IsTokenExpired)
            { 
                if (_updateCredentialsTask == null)
                { 
                    throw new CredentialsExpiredException();
                }
                updateTokenResult = 
                    _updateCredentialsTask.Execute(_credentials);
            }

            return updateTokenResult
                .ContinueWith(
                    task => _clientFactory
                        .CreateClient(typeof(TRequest), _credentials)
                        .GetDataPoints(_lastQuery)
                        .Result, 
                    TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task =>
                 {
                     if (task.Exception?.Flatten().InnerException is BadHttpRequestException)
                     {
                         throw new InvalidServiceRequestException<TService>();
                     }
                     return task.Result;
                 });
        }

        public void Handle(TokenUpdated<TService> @event)
        {
            _credentials = @event.Values.ToObject<TCredentials>();
        }
    }
}
