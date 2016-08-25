using System;
using Foundations;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
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
                    userId,
                    new HtmlSerializer())
        { }

        protected override bool IsResponseError(HttpValueCollection query)
        {
            return query.ContainsKey(
                OAuth2ParameterEnum.Error.EnumToString());
        }

        public override TToken ParseAndValidateCallback<TToken>(Uri responseUri)
        {
            var result = base.ParseAndValidateCallback<TToken>(responseUri);

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

        protected override string GetQuerystring(Uri uri)
        {
            if (!string.IsNullOrEmpty(uri.Fragment) && uri.Fragment != "#_=_")
            {
                return uri.Fragment.TrimStart('#');
            }

            return null;
        }
    }
}
