using System.Threading.Tasks;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Template
{
    public class OAuth1AuthenticationTemplate :
        OAuthAuthenticationTemplateBase<OAuth1Credentials>
    {
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        public OAuth1AuthenticationTemplate(
            IOAuthAuthorizerUI authorizerUI, 
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
                OAuth1ParameterEnum.OAuthTokenSecret.EnumToString());

            return _oauthFacade.GetAccessTokenFromCallbackResult(
                intermediateResult, 
                oauthSecret);
        }
    }
}
