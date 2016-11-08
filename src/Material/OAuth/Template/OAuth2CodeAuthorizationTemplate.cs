using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{
    public class OAuth2CodeAuthorizationTemplate : 
        OAuthAuthorizationTemplateBase<OAuth2Credentials>
    {
        private readonly string _clientSecret;

        public OAuth2CodeAuthorizationTemplate(
            IOAuthAuthorizerUI<OAuth2Credentials> authorizerUI, 
            IOAuthFacade<OAuth2Credentials> oauthFacade, 
            string clientSecret) : 
                base(
                    authorizerUI, 
                    oauthFacade)
        {
            _clientSecret = clientSecret;
        }

        protected override Task<OAuth2Credentials> GetAccessTokenFromIntermediateResult(
            OAuth2Credentials intermediateResult)
        {
            return oauthFacade.GetAccessTokenAsync(
                intermediateResult, 
                _clientSecret);
        }
    }
}
