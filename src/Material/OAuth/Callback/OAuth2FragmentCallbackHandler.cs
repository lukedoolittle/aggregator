using System;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth.Callback
{
    public class OAuth2FragmentCallbackHandler :
        OAuthCallbackHandlerBase<OAuth2Credentials>
    {
        public OAuth2FragmentCallbackHandler(
            IOAuthSecurityStrategy securityStrategy,
            string securityParameter) :
                base(
                    securityStrategy,
                    securityParameter,
                    new HtmlSerializer())
        { }

        protected override string GetQuerystring(Uri uri)
        {
            if (!string.IsNullOrEmpty(uri.Fragment) && uri.Fragment != "#_=_")
            {
                return uri.Fragment.TrimStart('#');
            }

            return null;
        }

        protected override bool IsResponseError(HttpValueCollection query)
        {
            return query.ContainsKey(
                OAuth2Parameter.Error.EnumToString());
        }
    }
}
