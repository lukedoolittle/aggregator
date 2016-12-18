using System;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;
using Material.OAuth.Authorization;

namespace Material.OAuth.Workflow
{
    public class OAuthRequester
    {
        private readonly IOAuthProtectedResourceAdapter _requester;
        private readonly string _userId;

        /// <summary>
        /// Request for a NoAuth (api key) endpoint
        /// </summary>
        /// <param name="credentials">Api key credentials used for authentication</param>
        public OAuthRequester(ApiKeyCredentials credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            _requester = new OAuthProtectedResourceAdapter(
                new AuthenticatorBuilder()
                    .AddParameter(
                        new ApiKey(
                            credentials.KeyName,
                            credentials.KeyValue,
                            credentials.KeyType)),
                HttpParameterType.Querystring);
        }

        /// <summary>
        /// OAuth requests for an OAuth1 endpoint
        /// </summary>
        /// <param name="credentials">OAuth1 credentials used for authentication</param>
        public OAuthRequester(OAuth1Credentials credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();

            _requester = new OAuthProtectedResourceAdapter(
                new AuthenticatorBuilder()
                    .AddParameter(new OAuth1ConsumerKey(credentials.ConsumerKey))
                    .AddParameter(new OAuth1Token(credentials.OAuthToken))
                    .AddParameter(new OAuth1Timestamp())
                    .AddParameter(new OAuth1Nonce())
                    .AddParameter(new OAuth1Version())
                    .AddParameter(new OAuth1SignatureMethod(signingAlgorithm))
                    .AddSigner(new OAuth1RequestSigningAlgorithm(
                        credentials.ConsumerSecret,
                        credentials.OAuthSecret,
                        signingAlgorithm)),
                HttpParameterType.Querystring);

            _userId = credentials.UserId;
        }

        /// <summary>
        /// OAuth requests for an OAuth2 endpoint
        /// </summary>
        /// <param name="credentials">OAuth2 credentials used for authentication</param>
        public OAuthRequester(OAuth2Credentials credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            _requester = new OAuthProtectedResourceAdapter(
                new AuthenticatorBuilder()
                    .AddParameter(
                        new OAuth2AccessToken(
                            credentials.AccessToken, 
                            credentials.TokenName)), 
                HttpParameterType.Querystring);

            _userId = credentials.UserId;
        }

        /// <summary>
        /// Get a protected resource from the authenticated provider
        /// </summary>
        /// <typeparam name="TRequest">Request to make to provider</typeparam>
        /// <typeparam name="TResponse">Protected resource</typeparam>
        /// <param name="request"></param>
        /// <returns>Protected resource from provider</returns>
        public Task<TResponse> MakeOAuthRequestAsync<TRequest, TResponse>(
            TRequest request)
            where TRequest : OAuthRequest, new()
        {
            if (request == null) { throw new ArgumentNullException(nameof(request));}

            request.AddUserIdParameter(_userId);

            return _requester
                    .ForProtectedResource<TRequest, TResponse>(
                        request);
        }

        /// <summary>
        /// Get a protected resource from the authenticated provider
        /// </summary>
        /// <typeparam name="TRequest">Request to make to provider</typeparam>
        /// <typeparam name="TResponse">Protected resource</typeparam>
        /// <returns>Protected resource from provider</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public Task<TResponse> MakeOAuthRequestAsync<TRequest, TResponse>()
            where TRequest : OAuthRequest, new()
        {
            return MakeOAuthRequestAsync<TRequest, TResponse>(
                new TRequest());
        }
    }
}

