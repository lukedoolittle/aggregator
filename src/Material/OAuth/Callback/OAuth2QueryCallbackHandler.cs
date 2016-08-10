using System;
using Foundations;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;

namespace Material.OAuth
{
    public class OAuth2QueryCallbackHandler : OAuthCallbackHandlerBase
    {
        public OAuth2QueryCallbackHandler(
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

        public override TToken ParseAndValidateCallback<TToken>(Uri responseUri)
        {
            var result = base.ParseAndValidateCallback<TToken>(responseUri);

            result?.AdditionalTokenParameters.Remove(
                OAuth2ParameterEnum.State.EnumToString());

            return result;
        }
    }

    public class OAuth2FragmentCallbackHandler : OAuth2QueryCallbackHandler
    {
        public OAuth2FragmentCallbackHandler(
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
