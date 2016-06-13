using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BatmansBelt.Serialization;
using Newtonsoft.Json.Linq;
using Aggregator.Framework;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Exceptions;
using Aggregator.Framework.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;

namespace Aggregator.Infrastructure.Authentication
{
    public class OAuth : IOAuth
    {
        private readonly IAuthenticator _authenticator;
        private readonly IDictionary<string, string> _additionalCredentialParameters;

        private OAuth(IDictionary<string, string> additionalCredentialParameters)
        {
            _additionalCredentialParameters = additionalCredentialParameters;
        }
         
        public OAuth(
            string accessToken, 
            string accessTokenName,
            IDictionary<string, string> additionalCredentialParameters) : 
            this(additionalCredentialParameters)
        {
            if (string.IsNullOrEmpty(accessTokenName))
            {
                throw new ArgumentNullException(nameof(accessTokenName));
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _authenticator = OAuth2Authenticator.ForProtectedResource(
                accessToken,
                accessTokenName);
        }

        public OAuth(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            OAuthParameterEnum parameterHandling,
            IDictionary<string, string> additionalCredentialParameters) :
            this(additionalCredentialParameters)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }

            if (string.IsNullOrEmpty(oauthToken))
            {
                throw new ArgumentNullException(nameof(oauthToken));
            }

            if (string.IsNullOrEmpty(oauthSecret))
            {
                throw new ArgumentNullException(nameof(oauthSecret));
            }
            
            var authenticator = OAuth1Authenticator.ForProtectedResource(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret);

            if (parameterHandling == OAuthParameterEnum.Querystring)
            {
                authenticator.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;
            }
            
            _authenticator = authenticator;
        }

		public async Task<JToken> ForProtectedResource(
            string baseUrl,
            string path,
            string httpMethod,
            string filter,
            Dictionary<string, string> headers,
            Dictionary<string, string> additionalQuerystringParameters, 
            Dictionary<string, string> additionalUrlSegmentParameters, 
            string recencyValue = "")
		{
		    if (string.IsNullOrEmpty(baseUrl))
		    {
		        throw new ArgumentNullException(nameof(baseUrl));
		    }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            var client = new RestClient(baseUrl)
		    {
		        Authenticator = _authenticator
		    };

		    var method = httpMethod ==
                WebRequestMethods.Http.Post ? Method.POST : Method.GET;

			var restRequest = new RestRequest(
				path,
				method);

		    if (headers != null)
		    {
		        foreach (var header in headers)
		        {
		            restRequest.AddHeader(header.Key, header.Value);
		        }
		    }

			if (additionalQuerystringParameters != null)
			{
				foreach (var parameter in additionalQuerystringParameters)
				{
				    if (string.IsNullOrEmpty(parameter.Value) && 
                        _additionalCredentialParameters.ContainsKey(parameter.Key))
				    {
                        restRequest.AddParameter(
                            parameter.Key, 
                            _additionalCredentialParameters[parameter.Key]);
                    }
				    else
				    {
                        restRequest.AddParameter(
                            parameter.Key, 
                            parameter.Value);
                    }
				}
			}

		    if (additionalUrlSegmentParameters != null)
		    {
		        foreach (var parameter in additionalUrlSegmentParameters)
		        {
                    restRequest.AddUrlSegment(parameter.Key, parameter.Value);
		        }
		    }

            if (filter != string.Empty &&
                !string.IsNullOrEmpty(recencyValue))
            {
                restRequest.AddParameter(
                    filter,
                    recencyValue);
            }

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException();
            }

            var response = await client.ExecuteTaskAsync(restRequest)
                .ConfigureAwait(false);

		    if (response.StatusCode != HttpStatusCode.OK)
		    {
		        throw new BadHttpRequestException(response.Content);
		    }

		    return response.Content.AsEntity<JToken>(false);
		}
    }
}
