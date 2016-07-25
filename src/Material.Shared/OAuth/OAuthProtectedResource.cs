using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Enums;
using Material.Exceptions;
using Material.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;

namespace Material.Infrastructure.OAuth
{
    public class OAuthProtectedResource : IOAuthProtectedResource
    {
        private readonly IAuthenticator _authenticator;
         
        public OAuthProtectedResource(
            string accessToken, 
            string accessTokenName) 
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

        public OAuthProtectedResource(
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            OAuthParameterTypeEnum parameterHandling) 
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

            if (parameterHandling == OAuthParameterTypeEnum.Querystring)
            {
                authenticator.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;
            }
            
            _authenticator = authenticator;
        }

		public async Task<string> ForProtectedResource(
            string baseUrl,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> querystringParameters, 
            IDictionary<string, string> pathParameters,
            HttpStatusCode expectedResponse = HttpStatusCode.OK)
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

		    foreach (var header in headers)
		    {
		        restRequest.AddHeader(
                    header.Key.ToString(), 
                    header.Value);
		    }

			foreach (var parameter in querystringParameters)
			{
			    restRequest.AddParameter(
			        parameter.Key,
			        parameter.Value);
			}

		    foreach (var parameter in pathParameters)
		    {
                restRequest.AddUrlSegment(
                    parameter.Key, 
                    parameter.Value);
		    }

            if (!Platform.IsOnline)
            {
                throw new ConnectivityException(
                    StringResources.OfflineConnectivityException);
            }

            var response = await client.ExecuteTaskAsync(restRequest)
                .ConfigureAwait(false);

		    if (response.StatusCode != expectedResponse)
		    {
		        throw new BadHttpRequestException(string.Format(
                    StringResources.BadHttpRequestException,
                    response.StatusCode,
                    response.Content));
		    }

		    return response.Content;
		}
    }
}
