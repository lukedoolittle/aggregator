using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{ 
    public abstract class OAuthAuthenticationTemplateBase<TCredentials> : 
        IOAuthAuthenticationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthAuthorizerUI<TCredentials> _authorizerUI;
        protected readonly IOAuthFacade<TCredentials> _oauthFacade;

        protected OAuthAuthenticationTemplateBase(
            IOAuthAuthorizerUI<TCredentials> authorizerUI, 
            IOAuthFacade<TCredentials> oauthFacade)
        {
            _authorizerUI = authorizerUI;
            _oauthFacade = oauthFacade;
        }

        public async Task<TCredentials> GetAccessTokenCredentials(string userId)  
        {
            var authorizationPath = await GetAuthorizationPath(userId)
                .ConfigureAwait(false);

            var intermediateResult = await GetIntermediateResult(
                    authorizationPath,
                    userId)
                .ConfigureAwait(false);

            return await GetAccessTokenFromIntermediateResult(
                    intermediateResult)
                .ConfigureAwait(false);
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

        protected abstract Task<TCredentials> GetAccessTokenFromIntermediateResult(
            TCredentials intermediateResult);
    }
}
