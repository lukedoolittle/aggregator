using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{
    public class OAuth1AuthorizationTemplate :
        OAuthAuthorizationTemplateBase<OAuth1Credentials>
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public OAuth1AuthorizationTemplate(
            IOAuthAuthorizerUI<OAuth1Credentials> authorizerUI, 
            IOAuthFacade<OAuth1Credentials> oauthFacade, 
            IOAuthSecurityStrategy securityStrategy, 
            string userId) : 
                base(
                    authorizerUI, 
                    oauthFacade)
        {
            _securityStrategy = securityStrategy;
            _userId = userId;
        }

        protected override Task<OAuth1Credentials> GetAccessTokenFromIntermediateResult(
            OAuth1Credentials intermediateResult)
        {
            var oauthSecret = _securityStrategy.CreateOrGetSecureParameter(
                _userId,
                OAuth1Parameter.OAuthTokenSecret.EnumToString());

            return oauthFacade.GetAccessTokenAsync(
                intermediateResult, 
                oauthSecret);
        }
    }
}
