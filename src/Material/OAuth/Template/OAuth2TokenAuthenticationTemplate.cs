using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth.Template
{
    public class OAuth2TokenAuthenticationTemplate :
        OAuthAuthenticationTemplateBase<OAuth2Credentials>
    {
        public OAuth2TokenAuthenticationTemplate(
            IOAuthAuthorizerUI<OAuth2Credentials> authorizerUI,
            IOAuthFacade<OAuth2Credentials> oauthFacade) :
                base(
                    authorizerUI,
                    oauthFacade)
        { }

        protected override Task<OAuth2Credentials> GetAccessTokenFromIntermediateResult(
            OAuth2Credentials intermediateResult)
        {
            return Task.FromResult(intermediateResult);
        }
    }
}
