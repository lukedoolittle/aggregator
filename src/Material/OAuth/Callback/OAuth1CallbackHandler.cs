using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Callback
{
    public class OAuth1CallbackHandler : OAuthCallbackHandlerBase<OAuth1Credentials>
    {
        public OAuth1CallbackHandler(
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
                OAuth1Parameter.Error.EnumToString());
        }
    }
}
