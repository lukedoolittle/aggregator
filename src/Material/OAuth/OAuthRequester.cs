using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;

namespace Material
{
    public class OAuthRequester
    {
        private readonly IOAuthProtectedResource _requester;
        private readonly string _userId;

        /// <summary>
        /// OAuth requests for an OAuth1 endpoint
        /// </summary>
        /// <param name="credentials">OAuth1 credentials used for authentication</param>
        public OAuthRequester(OAuth1Credentials credentials)
        {
            _requester = new OAuthProtectedResource(
                credentials.ConsumerKey,
                credentials.ConsumerSecret,
                credentials.OAuthToken,
                credentials.OAuthSecret,
                credentials.ParameterHandling);
            _userId = credentials.UserId;
        }

        /// <summary>
        /// OAuth requests for an OAuth2 endpoint
        /// </summary>
        /// <param name="credentials">OAuth2 credentials used for authentication</param>
        public OAuthRequester(OAuth2Credentials credentials)
        {
            _requester = new OAuthProtectedResource(
                credentials.AccessToken,
                credentials.TokenName);
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
            TRequest request = null)
            where TRequest : OAuthRequest, new()
        {
            if (request == null)
            {
                request = new TRequest();
            }

            request.AddUserIdParameter(_userId);

            return _requester
                    .ForProtectedResource<TResponse>(
                        request.Host,
                        request.Path,
                        request.HttpMethod,
                        request.Headers,
                        request.QuerystringParameters,
                        request.PathParameters,
                        request.Body,
                        request.BodyType);
        }
    }
}
