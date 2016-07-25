using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Requests;
using Foundations.Extensions;
using Foundations.Reflection;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Metadata;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Factories
{
    //TODO: convert this into Builder pattern
    public class SampleRequestTaskFactory : ISampleRequestTaskFactory
    {
        private readonly ICommandSender _sender;
        private readonly IEventPublisher _publisher;
        private readonly ISubscriptionManager _manager;
        private readonly IAuthenticationRefreshTaskFactory _factory;
        private readonly IClientFactory _clientFactory;

        public SampleRequestTaskFactory(
            ICommandSender sender,
            IEventPublisher publisher,
            ISubscriptionManager manager,
            IAuthenticationRefreshTaskFactory factory,
            IClientFactory clientFactory)
        {
            _sender = sender;
            _manager = manager;
            _factory = factory;
            _clientFactory = clientFactory;
            _publisher = publisher;
        }

        //TODO: why are credentials passed as a JObject?
        public virtual ITask GetTask(
            Guid aggregateId,
            JObject credentials,
            Type requestType,
            string lastQuery)
        {
            if (!requestType.HasBase(typeof(Request)))
            {
                throw new NotSupportedException();
            }

            if (requestType == typeof(SMSRequest) || 
                requestType == typeof(GPSRequest))
            {
                return new MethodInvoker(this, "GetOnboardRequestTask")
                    .AddGenericParameter(requestType)
                    .AddMethodParameter(aggregateId)
                    .AddMethodParameter(lastQuery)
                    .Execute<ITask>();
            }

            if (requestType.HasBase(typeof(OAuthRequest)) ||
                requestType.HasBase(typeof(BluetoothRequest)))
            {
                var serviceType = requestType
                    .GetCustomAttributes<ServiceType>()
                    .Single()
                    .Type;
                var credentialType = serviceType
                    .GetCustomAttributes<CredentialType>()
                    .Single()
                    .Type;

                ITask refreshCredentialsTask = null;

                if (serviceType.HasBase(typeof(OAuth2ResourceProvider)))
                {
                    refreshCredentialsTask = new MethodInvoker(
                            _factory, 
                            "GenerateRefreshTask")
                        .AddGenericParameter(serviceType)
                        .AddMethodParameter(aggregateId)
                        .Execute<ITask>();
                }

                var task = new MethodInvoker(
                        this, 
                        "GetSampleRequestTask")
                    .AddGenericParameter(requestType)
                    .AddGenericParameter(credentialType)
                    .AddGenericParameter(serviceType)
                    .AddMethodParameter(aggregateId)
                    .AddMethodParameter(lastQuery)
                    .AddMethodParameter(credentials)
                    .AddMethodParameter(refreshCredentialsTask)
                    .Execute<ITask>();

                new MethodInvoker(this, "SubscribeTask")
                    .AddGenericParameter(requestType)
                    .AddGenericParameter(credentialType)
                    .AddGenericParameter(serviceType)
                    .AddMethodParameter(task)
                    .Execute();

                return task;
            }

            throw new NotSupportedException();
        }

        private ITask GetOnboardRequestTask<TRequest>(
            Guid aggregateId,
            string lastQuery)
            where TRequest : Request, new()
        {
            var client = _clientFactory.CreateClient<TRequest>();

            return new OnboardRequestTask<TRequest>(
                _publisher, 
                _sender, 
                aggregateId, 
                lastQuery, 
                client);
        }

        private ITask GetSampleRequestTask<TRequest, TCredentials, TService>(
            Guid aggregateId,
            string lastQuery,
            JObject credentials,
            ITask refreshCredentialsTask)
            where TRequest : Request, new()
            where TCredentials : TokenCredentials
            where TService : ResourceProvider
        {
            var credential = credentials.ToObject<TCredentials>();

            return new SampleRequestTask<TRequest, TCredentials, TService>(
                _clientFactory, 
                _sender, 
                _publisher,
                credential, 
                refreshCredentialsTask, 
                aggregateId, 
                lastQuery);
        }

        private void SubscribeTask<TRequest, TCredentials, TService>(SampleRequestTask<TRequest, TCredentials, TService> task)
            where TRequest : Request, new()
            where TService : ResourceProvider
            where TCredentials : TokenCredentials
        {
            _manager.Subscribe<FilterChanged<TRequest>>(task.Handle);

            _manager.Subscribe<TokenUpdated<TService>>(task.Handle);
        }
    }
}
