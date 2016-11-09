using System;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth.Callback
{
    public class OAuth1CallbackHandler : 
        OAuthCallbackHandlerBase<OAuth1Credentials>
    {
        public OAuth1CallbackHandler(
            IOAuthSecurityStrategy securityStrategy, 
            string securityParameter) :
                base(
                    securityStrategy,
                    securityParameter, 
                    new HtmlSerializer())
        { }

        protected override bool IsResponseError(
            HttpValueCollection query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return query.ContainsKey(
                    OAuth1Parameter.Error.EnumToString()) || 
                query.Count == 0;
        }

        protected override bool IsDiscardableResponse(
            HttpValueCollection query)
        {
            return false;
        }
    }
}
