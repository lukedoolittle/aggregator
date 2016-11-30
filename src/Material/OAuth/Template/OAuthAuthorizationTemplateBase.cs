using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{ 
    public abstract class OAuthAuthorizationTemplateBase<TCredentials> : 
        IOAuthAuthorizationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthAuthorizerUI<TCredentials> _authorizerUI;
        protected IOAuthFacade<TCredentials> OauthFacade { get; }

        protected OAuthAuthorizationTemplateBase(
            IOAuthAuthorizerUI<TCredentials> authorizerUI, 
            IOAuthFacade<TCredentials> oauthFacade)
        {
            _authorizerUI = authorizerUI;
            OauthFacade = oauthFacade;
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
                        intermediateResult)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        protected virtual Task<Uri> GetAuthorizationPath(string userId)
        {
            return OauthFacade.GetAuthorizationUriAsync(userId);
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
