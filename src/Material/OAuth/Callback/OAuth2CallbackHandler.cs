using System;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Callback
{
    public class OAuth2CallbackHandler : OAuthCallbackHandlerBase<OAuth2Credentials>
    {
        public OAuth2CallbackHandler(
            IOAuthSecurityStrategy securityStrategy,
            string securityParameter) :
                base(
                    securityStrategy,
                    securityParameter,
                    new HtmlSerializer())
        { }

        protected override HttpValueCollection GetQuerystring(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var query = base.GetQuerystring(uri);

            query.Add(HttpUtility.ParseQueryString(uri.Fragment));

            return query;
        }

        protected override bool IsResponseError(HttpValueCollection query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return query.ContainsKey(
                OAuth2Parameter.Error.EnumToString());
        }
    }
}
