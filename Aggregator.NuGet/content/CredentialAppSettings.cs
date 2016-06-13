using System;
using System.Collections.Generic;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Exceptions;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Services;

namespace Aggregator.Configuration
{
    public class CredentialApplicationSettings : IClientCredentials
    {
		//POPULATE DESIRED SERVICES WITH CLIENT CREDENTIALS
        private static readonly Dictionary<Type, TokenCredentials> _credentials = new Dictionary<Type, TokenCredentials>
        {
            { typeof(Facebook), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Foursquare), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Spotify), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Google), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Linkedin), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Rescuetime), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Runkeeper), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Fitbit), new OAuth2Credentials().SetClientProperties("", "") },
            { typeof(Withings), new OAuth1Credentials().SetConsumerProperties("", "") },
            { typeof(Twitter), new OAuth1Credentials().SetConsumerProperties("", "") },
            { typeof(Fatsecret), new OAuth1Credentials().SetConsumerProperties("", "") },
        };

        public TCredentials GetClientCredentials<TService, TCredentials>()
            where TService : Service
            where TCredentials : TokenCredentials
        {
            TokenCredentials credentials = null;
            if(_credentials.TryGetValue(typeof(TService), out credentials))
            {
                if (!credentials.HasValidProperties)
                {
                    throw new CredentialsNotPopulatedException(typeof(TService).Name);
                }

                return credentials as TCredentials;
            }
            else
            {
                throw new CredentialsDoNotExistException(typeof(TService).Name);
            }
        }
    }
}
