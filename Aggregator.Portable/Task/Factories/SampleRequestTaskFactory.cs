using System;
using System.Linq;
using BatmansBelt.Extensions;
using BatmansBelt.Reflection;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Aggregator.Framework.Metadata;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Clients;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Requests;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Factories
{
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

            var clientType = requestType
                .GetCustomAttributes<ClientType>()
                .Single()
                .Type;

            IRequestClient client = null;

            if (clientType == typeof (SMSClient))
            {
                client = _clientFactory.CreateClient(requestType);
            }
            if (clientType == typeof(GPSClient))
            {
                client = _clientFactory.CreateClient(requestType);
            }
            if (client != null)
            {
                return new InstanceCreator(typeof(OnboardRequestTask<>))
                    .AddGenericParameter(requestType)
                    .AddConstructorParameter(_sender)
                    .AddConstructorParameter(aggregateId)
                    .AddConstructorParameter(lastQuery)
                    .AddConstructorParameter(client)
                    .Create<ITask>();
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
                var refreshCredentialsTask = _factory
                            .GenerateRefreshTask(serviceType, aggregateId);
                var credential = credentials.ToObject(credentialType);

                var task = new InstanceCreator(typeof(SampleRequestTask<,,>))
                    .AddGenericParameter(requestType)
                    .AddGenericParameter(credentialType)
                    .AddGenericParameter(serviceType)
                    .AddConstructorParameter(_clientFactory)
                    .AddConstructorParameter(_sender)
                    .AddConstructorParameter(_publisher)
                    .AddConstructorParameter(credential)
                    .AddConstructorParameter(refreshCredentialsTask)
                    .AddConstructorParameter(aggregateId)
                    .AddConstructorParameter(lastQuery)
                    .Create<ITask>();

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

        private void SubscribeTask<TRequest, TCredentials, TService>(SampleRequestTask<TRequest, TCredentials, TService> task)
            where TRequest : Request, new()
            where TService : Service
            where TCredentials : TokenCredentials
        {
            _manager.Subscribe<FilterChanged<TRequest>>(task.Handle);

            _manager.Subscribe<TokenUpdated<TService>>(task.Handle);
        }
    }
}
