using System;
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

    public class OAuth2TokenCallbackHandler : OAuth2CallbackHandler
    {
        public OAuth2TokenCallbackHandler(
            IOAuthSecurityStrategy securityStrategy, 
            string securityParameter, 
            string userId) : 
                base(
                    securityStrategy, 
                    securityParameter, 
                    userId)
        { }

        protected override HttpValueCollection ParseQuerystring(Uri uri)
        {
            if (!string.IsNullOrEmpty(uri.Fragment) && uri.Fragment != "#_=_")
            {
                return HttpUtility.ParseQueryString(uri.Fragment);
            }

            return null;
        }
    }
}
