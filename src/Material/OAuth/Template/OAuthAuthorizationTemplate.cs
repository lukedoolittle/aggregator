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
        private readonly IOAuthFacade<TCredentials> _oauthFacade;

        public OAuthAuthorizationTemplate(
            IOAuthAuthorizerUI<TCredentials> authorizerUI, 
            IOAuthFacade<TCredentials> oauthFacade)
        {
            _authorizerUI = authorizerUI;
            _oauthFacade = oauthFacade;
        }

        public async Task<TCredentials> GetAccessTokenCredentials(string userId)  
        {
            try
            {
                var authorizationPath = await _oauthFacade
                    .GetAuthorizationUriAsync(userId)
                    .ConfigureAwait(false);

                var intermediateResult = await _authorizerUI
                    .Authorize(
                        authorizationPath,
                        userId)
                    .ConfigureAwait(false);

                return await _oauthFacade
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
