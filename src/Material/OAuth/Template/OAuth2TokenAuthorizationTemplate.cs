using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{
    public class OAuth2TokenAuthorizationTemplate :
        OAuthAuthorizationTemplateBase<OAuth2Credentials>
    {
        public OAuth2TokenAuthorizationTemplate(
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
