using System.Threading.Tasks;
using Foundations.Serialization;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Task;

namespace Material
{
    public class OAuthRequester
    {
        private readonly IOAuthProtectedResource _requester;
        private readonly string _userId;

        public OAuthRequester(OAuth1Credentials credentials)
        {
            var factory = new OAuthFactory();
            _requester = factory.GetOAuth(credentials);
            _userId = credentials.UserId;
        }

        public OAuthRequester(OAuth2Credentials credentials)
        {
            var factory = new OAuthFactory();
            _requester = factory.GetOAuth(credentials);
            _userId = credentials.UserId;
        }

        public async Task<TResponse> MakeOAuthRequestAsync<TRequest, TResponse>(
            TRequest request = null)
            where TRequest : OAuthRequest, new()
        {
            if (request == null)
            {
                request = new TRequest();
            }

            request.AddUserIdParameter(_userId);

            var result = await _requester
                .ForProtectedResource(
                    request.Host,
                    request.Path,
                    request.HttpMethod,
                    request.Headers,
                    request.QuerystringParameters,
                    request.PathParameters)
                .ConfigureAwait(false);

            return result.AsEntity<TResponse>(false);
        }
    }
}
