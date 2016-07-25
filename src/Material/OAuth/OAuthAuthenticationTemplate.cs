using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuthAuthenticationTemplate<TCredentials> : 
        IOAuthAuthenticationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IOAuthAuthorizerUI _authorizerUI;
        private readonly IOAuthFacade<TCredentials> _oauthFacade;

        public OAuthAuthenticationTemplate(
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

        protected virtual Task<TCredentials> GetAccessTokenFromIntermediateResult(
            TCredentials intermediateResult)
        {
            return _oauthFacade
                .GetAccessTokenFromCallbackResult(intermediateResult);
        }
    }

    public class OAuthTokenAuthenticationTemplate<TCredentials> :
        OAuthAuthenticationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        public OAuthTokenAuthenticationTemplate(
            IOAuthAuthorizerUI authorizerUI,
            IOAuthFacade<TCredentials> oauthFacade) :
            base(authorizerUI, oauthFacade)
        {}

        protected override Task<TCredentials> GetAccessTokenFromIntermediateResult(
            TCredentials intermediateResult)
        {
            return Task.FromResult(intermediateResult);
        }
    }
}
