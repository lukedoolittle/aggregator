using System;
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
                var authorizationPath = await GetAuthorizationPath(userId)
                    .ConfigureAwait(false);

                var intermediateResult = await GetIntermediateResult(
                        authorizationPath,
                        userId)
                    .ConfigureAwait(false);

                return await GetAccessTokenFromIntermediateResult(
                        intermediateResult,
                        userId)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        protected virtual Task<Uri> GetAuthorizationPath(string userId)
        {
            return _oauthFacade.GetAuthorizationUriAsync(userId);
        }

        protected virtual Task<TCredentials> GetIntermediateResult(
            Uri authorizationPath,
            string userId)
        {
            return _authorizerUI.Authorize(
                authorizationPath,
                userId);
        }

        protected virtual Task<TCredentials> GetAccessTokenFromIntermediateResult(
            TCredentials intermediateResult,
            string userId)
        {
            return _oauthFacade.GetAccessTokenAsync(
                intermediateResult, 
                userId);
        }
    }
}
