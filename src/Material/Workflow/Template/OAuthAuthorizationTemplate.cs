using System.Threading.Tasks;
using Material.Contracts;
using Material.Domain.Credentials;

namespace Material.Workflow.Template
{ 
    public class OAuthAuthorizationTemplate<TCredentials> : 
        IOAuthAuthorizationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthAuthorizerUI<TCredentials> _authorizerUI;
        private readonly IOAuthAuthorizationUriFacade _uriFacade;
        private readonly IOAuthAccessTokenFacade<TCredentials> _accessTokenFacade;
        private readonly IOAuthSecurityStrategy _securityStrategy;

        public OAuthAuthorizationTemplate(
            IOAuthAuthorizerUI<TCredentials> authorizerUI,
            IOAuthAuthorizationUriFacade uriFacade, 
            IOAuthAccessTokenFacade<TCredentials> accessTokenFacade, 
            IOAuthSecurityStrategy securityStrategy)
        {
            _authorizerUI = authorizerUI;
            _uriFacade = uriFacade;
            _accessTokenFacade = accessTokenFacade;
            _securityStrategy = securityStrategy;
        }

        public async Task<TCredentials> GetAccessTokenCredentials(
            string requestId)  
        {
            try
            {
                var authorizationPath = await _uriFacade
                    .GetAuthorizationUriAsync(requestId)
                    .ConfigureAwait(false);

                var intermediateResult = await _authorizerUI
                    .Authorize(
                        authorizationPath,
                        requestId)
                    .ConfigureAwait(false);

                return await _accessTokenFacade
                    .GetAccessTokenAsync(
                        intermediateResult,
                        requestId)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                _securityStrategy.ClearSecureParameters(requestId);

                return null;
            }
        }
    }
}
