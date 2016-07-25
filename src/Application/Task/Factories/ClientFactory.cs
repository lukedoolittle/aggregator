using System;
using Aggregator.Infrastructure.Clients;
using Foundations.Extensions;
using Foundations.Reflection;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using WithingsWeighin = Material.Infrastructure.Requests.WithingsWeighin;

namespace Aggregator.Task.Factories
{
    public class ClientFactory : IClientFactory
    {
        private readonly IBluetoothAdapter _bluetoothManager;
        private readonly IGPSAdapter _igpsAdapter;
        private readonly ISMSAdapter _smsManager;
        private readonly IOAuthFactory _oauthFactory;

        public ClientFactory(
            IBluetoothAdapter bluetoothManager, 
            IGPSAdapter igpsAdapter, 
            IOAuthFactory oauthFactory)
            : this(oauthFactory)
        {
            _bluetoothManager = bluetoothManager;
            _igpsAdapter = igpsAdapter;
        }        

        public ClientFactory(
            ISMSAdapter smsManager,
            IBluetoothAdapter bluetoothManager, 
            IGPSAdapter igpsAdapter, 
            IOAuthFactory oauthFactory) : 
            this(
                bluetoothManager, 
                igpsAdapter, 
                oauthFactory)
        {
            _smsManager = smsManager;
        }

        public ClientFactory(IOAuthFactory oauthFactory)
        {
            _oauthFactory = oauthFactory;
        }

        public IRequestClient CreateClient<TRequest, TCredentials>(
            TCredentials credentials) 
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

            if (requestType.HasBase(typeof(BluetoothRequest)))
            {
                return new InstanceCreator(typeof(BluetoothClient<>))
                    .AddGenericParameter(requestType)
                    .AddConstructorParameter(_bluetoothManager)
                    .AddConstructorParameter(credentials)
                    .Create<IRequestClient>();
            }

            var oauth = GetProtectedResource(credentials);

            if (requestType == typeof(FitbitIntradaySteps))
            {
                return new FitbitIntradayStepsOAuthClient(oauth);
            }
            else if (requestType == typeof(FitbitIntradayHeartRate))
            {
                return new FitbitIntradayHeartRateOAuthClient(oauth);
            }
            else if (requestType == typeof(FitbitSleep))
            {
                return new FitbitSleepOAuthClient(oauth);
            }
            else if (requestType == typeof(GoogleGmail))
            {
                return new GoogleGmailRecursiveOAuthClient(oauth);
            }
            else if (requestType == typeof(RescuetimeAnalyticData))
            {
                return new RescuetimeOAuthClient(oauth);
            }
            else if (requestType == typeof(WithingsWeighin))
            {
                return new WithingsWeighinOAuthClient(oauth);
            }
            else
            {
                if (requestType.IsInstantiableConcreteImplementation<ITimeseries>())
                {
                    return new InstanceCreator(typeof(TimeseriesOAuthClient<>))
                        .AddGenericParameter(requestType)
                        .AddConstructorParameter(oauth)
                        .Create<IRequestClient>();
                }
                else
                {
                    return new InstanceCreator(typeof(OAuthClient<>))
                        .AddGenericParameter(requestType)
                        .AddConstructorParameter(oauth)
                        .Create<IRequestClient>();
                }
            }
        }

        private IOAuthProtectedResource GetProtectedResource(object credentials)
        {
            if (credentials is OAuth1Credentials)
            {
                return _oauthFactory.GetOAuth(credentials as OAuth1Credentials);
            }
            else if (credentials is OAuth2Credentials)
            {
                return _oauthFactory.GetOAuth(credentials as OAuth2Credentials);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public IRequestClient CreateClient<TRequest>() 
            where TRequest : Request
        {
            var requestType = typeof(TRequest);

            if (requestType == typeof(SMSRequest))
            {
                return new SMSClient(_smsManager);
            }
            if (requestType == typeof(GPSRequest))
            {
                return new GPSClient(_igpsAdapter);
            }

            throw new NotSupportedException();
        }
    }
}
