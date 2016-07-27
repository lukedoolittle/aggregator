using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public abstract class OAuthAuthenticationTemplateBase<TCredentials> : 
        IOAuthAuthenticationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthAuthorizerUI _authorizerUI;
        protected readonly IOAuthFacade<TCredentials> _oauthFacade;

        protected OAuthAuthenticationTemplateBase(
            IOAuthAuthorizerUI authorizerUI, 
            IOAuthFacade<TCredentials> oauthFacade)
        {
            _authorizerUI = authorizerUI;
            _oauthFacade = oauthFacade;
        }

        public async Task<TCredentials> GetAccessTokenCredentials()  
        {
            var authorizationPath = await GetAuthorizationPath()
                .ConfigureAwait(false);

            var intermediateResult = await GetIntermediateResult(
                    authorizationPath)
                .ConfigureAwait(false);

            return await GetAccessTokenFromIntermediateResult(
                    intermediateResult)
                .ConfigureAwait(false);
        }

        protected virtual Task<Uri> GetAuthorizationPath()
        {
            return _oauthFacade.GetAuthorizationUri();
        }

        protected virtual Task<TCredentials> GetIntermediateResult(
            Uri authorizationPath)
        {
            //TODO: move the CallbackUri property out of IOAuthFacade and into IOAuthAuthorizerUI: makes more sense there
            return _authorizerUI.Authorize<TCredentials>(
                _oauthFacade.CallbackUri,
                authorizationPath);
        }

        protected abstract Task<TCredentials> GetAccessTokenFromIntermediateResult(
            TCredentials intermediateResult);
    }
}
