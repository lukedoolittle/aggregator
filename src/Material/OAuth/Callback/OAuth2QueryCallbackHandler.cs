using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2QueryCallbackHandler : 
        OAuthCallbackHandlerBase<OAuth2Credentials>
    {
        public OAuth2QueryCallbackHandler(
            IOAuthSecurityStrategy securityStrategy, 
            string securityParameter) : 
                base(
                    securityStrategy, 
                    securityParameter, 
                    new HtmlSerializer())
        { }

        protected override bool IsResponseError(HttpValueCollection query)
        {
            return query.ContainsKey(
                OAuth2ParameterEnum.Error.EnumToString());
        }
    }
}
