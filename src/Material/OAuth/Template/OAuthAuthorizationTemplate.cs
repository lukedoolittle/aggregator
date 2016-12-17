using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{ 
    public class OAuthAuthorizationTemplate<TCredentials> : 
        IOAuthAuthorizationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthAuthorizerUI<TCredentials> _authorizerUI;
        private readonly IOAuthAuthorizationUriFacade _uriFacade;
        private readonly IOAuthAccessTokenFacade<TCredentials> _accessTokenFacade;

        public OAuthAuthorizationTemplate(
            IOAuthAuthorizerUI<TCredentials> authorizerUI,
            IOAuthAuthorizationUriFacade uriFacade, 
            IOAuthAccessTokenFacade<TCredentials> accessTokenFacade)
        {
            _authorizerUI = authorizerUI;
            _uriFacade = uriFacade;
            _accessTokenFacade = accessTokenFacade;
        }

        public async Task<TCredentials> GetAccessTokenCredentials(string userId)  
        {
            try
            {
                var authorizationPath = await _uriFacade
                    .GetAuthorizationUriAsync(userId)
                    .ConfigureAwait(false);

                var intermediateResult = await _authorizerUI
                    .Authorize(
                        authorizationPath,
                        userId)
                    .ConfigureAwait(false);

                return await _accessTokenFacade
                    .GetAccessTokenAsync(
                        intermediateResult,
                        userId)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }
    }
}
