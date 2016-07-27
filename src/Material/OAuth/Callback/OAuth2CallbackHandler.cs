using Foundations;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;

namespace Material.OAuth
{
    public class OAuth2CallbackHandler : OAuthCallbackHandlerBase
    {
        public OAuth2CallbackHandler(
            IOAuthSecurityStrategy securityStrategy, 
            string securityParameter, 
            string userId) : 
                base(
                    securityStrategy, 
                    securityParameter, 
                    userId)
        { }

        protected override bool IsResponseError(HttpValueCollection query)
        {
            return query.ContainsKey(
                OAuth2ParameterEnum.Error.EnumToString());
        }
    }
}
