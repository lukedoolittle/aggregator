using System;
using System.Collections.Generic;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;

namespace Application.Configuration
{
    using Twitter = Material.Infrastructure.ProtectedResources.Twitter;

    public class CredentialApplicationSettings : IClientCredentials
    {
        private const string REDIRECT_URI_FORMAT = 
            "http://localhost:33533/oauth/{0}";

        private readonly Dictionary<Type, OAuth2Credentials> _jwtCredentials =
            new Dictionary<Type, OAuth2Credentials>
        {
                { typeof(Google), new OAuth2Credentials()
                                    .SetClientProperties(
                                        null,
                    @"-----BEGIN PRIVATE KEY-----

                    -----END PRIVATE KEY-----"
                    , "")
                }
        };

        private readonly Dictionary<Type, TokenCredentials> _protocolCredentials = 
            new Dictionary<Type, TokenCredentials>
        {
#if __ANDROID__
                { typeof(Google), new OAuth2Credentials()
                .SetClientProperties("", null)
                .SetCallbackUrl("quantfabric.material:/google")},
#else
                { typeof(Google), new OAuth2Credentials()
                .SetClientProperties("", null)
                .SetCallbackUrl("quantfabric.material:/google")},
#endif

                { typeof(Fitbit), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl("quantfabric.material://fitbit")}
        };

        private readonly Dictionary<Type, TokenCredentials> _localhostCredentials = 
            new Dictionary<Type, TokenCredentials>
        {
            { typeof(Facebook), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Facebook).Name.ToLower()))},

            { typeof(Foursquare), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Foursquare).Name.ToLower()))},

            { typeof(Spotify), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Spotify).Name.ToLower()))},

            { typeof(Google), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Google).Name.ToLower()))},

            { typeof(LinkedIn), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(LinkedIn).Name.ToLower()))},

            { typeof(Rescuetime), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl("https://localhost:33533/oauth/rescuetime")},

            { typeof(Runkeeper), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Runkeeper).Name.ToLower()))},

            { typeof(Fitbit), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Fitbit).Name.ToLower()))},

            { typeof(Withings), new OAuth1Credentials()
                .SetConsumerProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Withings).Name.ToLower()))},

            { typeof(Twitter), new OAuth1Credentials()
                .SetConsumerProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Twitter).Name.ToLower()))},

            { typeof(Fatsecret), new OAuth1Credentials()
                .SetConsumerProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Fatsecret).Name.ToLower()))},

            { typeof(TwentyThreeAndMe), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(TwentyThreeAndMe).Name.ToLower()))},

            { typeof(Omniture), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Omniture).Name.ToLower()))},

            { typeof(Pinterest), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl("https://localhost:33533/oauth/pinterest")},

            { typeof(Instagram), new OAuth2Credentials()
                .SetClientProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Instagram).Name.ToLower()))},

             { typeof(Tumblr), new OAuth1Credentials()
                .SetConsumerProperties("", "")
                .SetCallbackUrl(string.Format(REDIRECT_URI_FORMAT,typeof(Tumblr).Name.ToLower()))},
        };

        public TCredentials GetClientCredentials<TService, TCredentials>(
            CallbackTypeEnum callbackType = CallbackTypeEnum.Localhost)
            where TService : ResourceProvider
            where TCredentials : TokenCredentials
        {
            var credentialDictionary = callbackType == CallbackTypeEnum.Localhost
                ? _localhostCredentials
                : _protocolCredentials;

            TokenCredentials credentials = null;
            if (credentialDictionary.TryGetValue(typeof(TService), out credentials))
            {
                if (!credentials.HasValidPublicKey)
                {
                    throw new Exception($"{typeof(TService).Name} doesn't have a valid public key");
                }

                return (credentials as TCredentials);
            }
            else
            {
                throw new Exception($"{typeof(TService).Name} doesn't have populated credentials");
            }
        }

        public OAuth2Credentials GetJWTCredentials<TService>()
            where TService : ResourceProvider
        {
            OAuth2Credentials credentials = null;
            if (_jwtCredentials.TryGetValue(typeof(TService), out credentials))
            {
                return credentials;
            }
            else
            {
                throw new Exception($"{typeof(TService).Name} doesn't have populated credentials");
            }
        }
    }
}
