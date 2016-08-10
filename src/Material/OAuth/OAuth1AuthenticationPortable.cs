using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuth1AuthenticationPortable : IOAuth1Authentication
    {
        public async Task<OAuth1Credentials> GetRequestToken(
            Uri requestUrl, 
            string consumerKey, 
            string consumerSecret, 
            OAuthParameterTypeEnum parameterHandling,
            Uri callbackUrl)
        {
            if (requestUrl == null)
            {
                throw new ArgumentNullException(nameof(requestUrl));
            }

            if (callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }

            var request = new HttpRequest(requestUrl.NonPath())
                .PostTo(requestUrl.AbsolutePath)
                .ForOAuth1RequestToken(
                    consumerKey,
                    consumerSecret,
                    callbackUrl.ToString());

            if (parameterHandling == OAuthParameterTypeEnum.Body)
            {
                request.WithQueryParameters();
            }

            var response = await request
                .ExecuteAsync()
                .ConfigureAwait(false);

            var credentials = await response
                .ContentAsync<OAuth1Credentials>()
                .ConfigureAwait(false);

            return credentials.SetConsumerProperties(
                consumerKey, 
                consumerSecret);
        }

        public Uri GetAuthorizationUri(
            string oauthToken,
            Uri authorizeUri)
        {
            if (authorizeUri == null)
            {
                throw new ArgumentNullException(nameof(authorizeUri));
            }

            if (string.IsNullOrEmpty(oauthToken))
            {
                throw new ArgumentNullException(nameof(authorizeUri));
            }

            var builder = new UriBuilder(authorizeUri.NonPath());
            builder.Path += authorizeUri.AbsolutePath.TrimStart('/');

            var query = new HttpValueCollection
            {
                { OAuth2ParameterEnum.OAuthToken.EnumToString(), oauthToken}
            };

            builder.Query += query.ToString();

            return builder.Uri;
        }

        public Task<OAuth1Credentials> GetAccessToken(
            Uri accessUri, 
            string consumerKey, 
            string consumerSecret, 
            string oauthToken, 
            string oauthSecret,
            string verifier,
            OAuthParameterTypeEnum parameterHandling, 
            IDictionary<string, string> queryParameters)
        {
            throw new NotImplementedException();
        }
    }
}
