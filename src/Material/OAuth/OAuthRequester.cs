using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth;

namespace Material
{
    public class OAuthRequester
    {
        private readonly IOAuthProtectedResource _requester;
        private readonly string _userId;

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

        public OAuthRequester(OAuth2Credentials credentials)
        {
            _requester = new OAuthProtectedResource(
                credentials.AccessToken,
                credentials.TokenName);
            _userId = credentials.UserId;
        }

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
                        request.PathParameters);
        }
    }
}
