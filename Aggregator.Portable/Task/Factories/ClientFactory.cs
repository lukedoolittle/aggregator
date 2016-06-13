using System;
using System.Linq;
using BatmansBelt.Extensions;
using BatmansBelt.Reflection;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Metadata;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Clients;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Task.Factories
{
    public class ClientFactory : IClientFactory
    {
        private readonly IBluetoothManager _bluetoothManager;
        private readonly IGPSManager _gpsManager;
        private readonly ISMSManager _smsManager;
        private readonly IOAuthFactory _oauthFactory;

        public ClientFactory(
            IBluetoothManager bluetoothManager, 
            IGPSManager gpsManager, 
            IOAuthFactory oauthFactory)
            : this(oauthFactory)
        {
            _bluetoothManager = bluetoothManager;
            _gpsManager = gpsManager;
        }        

        public ClientFactory(
            ISMSManager smsManager,
            IBluetoothManager bluetoothManager, 
            IGPSManager gpsManager, 
            IOAuthFactory oauthFactory) : 
            this(
                bluetoothManager, 
                gpsManager, 
                oauthFactory)
        {
            _smsManager = smsManager;
        }

        public ClientFactory(IOAuthFactory oauthFactory)
        {
            _oauthFactory = oauthFactory;
        }

        public IRequestClient CreateClient<TRequest>(object credentials) 
            where TRequest : Request
        {
            return CreateClient(typeof (TRequest), credentials);
        }

        public IRequestClient CreateClient(
            Type requestType,
            object credentials) 
        {
            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }
            var clientType = requestType
                .GetCustomAttributes<ClientType>()
                .Single()
                .Type;
            var credentialType = credentials.GetType();

            if (requestType.HasBase(typeof(BluetoothRequest)))
            {
                return new InstanceCreator(clientType)
                    .AddConstructorParameter(_bluetoothManager)
                    .AddConstructorParameter(credentials)
                    .Create<IRequestClient>();
            }
            if (clientType == typeof (TwitterIdOAuthClient))
            {
                var oauth = _oauthFactory.GetOAuth(credentials as OAuth1Credentials);
                return new TwitterIdOAuthClient(oauth, new OAuthClient<TwitterId>(oauth));
            }

            if (clientType == typeof (GoogleGmailRecursiveOAuthClient))
            {
                var oauth = _oauthFactory.GetOAuth(credentials as OAuth2Credentials);
                return new GoogleGmailRecursiveOAuthClient(oauth, new OAuthClient<GoogleGmailMetadata>(oauth));
            }

            var authenticatedClientType = typeof(OAuthClient<>).WithGenericParameters(requestType);

            if (clientType == authenticatedClientType ||
                clientType.HasBase(authenticatedClientType))
            {
                IOAuth oauth = null;

                if (credentialType == typeof (OAuth1Credentials))
                {
                    oauth = _oauthFactory.GetOAuth(credentials as OAuth1Credentials);
                }
                else if (credentialType == typeof (OAuth2Credentials))
                {
                    oauth = _oauthFactory.GetOAuth(credentials as OAuth2Credentials);
                }
                else
                {
                    throw new NotSupportedException();
                }

                return new InstanceCreator(clientType)
                    .AddConstructorParameter(oauth)
                    .Create<IRequestClient>();
            }

            throw new NotSupportedException();
        }

        public IRequestClient CreateClient(Type requestType)
        {
            var clientType = requestType
                .GetCustomAttributes<ClientType>()
                .Single()
                .Type;

            if (clientType == typeof (SMSClient))
            {
                return new SMSClient(_smsManager);
            }
            if (clientType == typeof (GPSClient))
            {
                return new GPSClient(_gpsManager);
            }

            throw new NotSupportedException();
        }
    }
}
