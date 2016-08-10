using Foundations;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;

namespace Material.OAuth
{
    public class OAuth1CallbackHandler : OAuthCallbackHandlerBase
    {
        public OAuth1CallbackHandler(
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
                OAuth1ParameterEnum.Error.EnumToString());
        }
    }
}
