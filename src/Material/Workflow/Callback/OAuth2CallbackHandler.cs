using System;
using Material.Domain.Credentials;
using Material.Contracts;
using Material.Framework.Collections;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.Framework.Serialization;

namespace Material.Workflow.Callback
{
    public class OAuth2CallbackHandler : OAuthCallbackHandlerBase<OAuth2Credentials>
    {
        public OAuth2CallbackHandler(
            IOAuthSecurityStrategy securityStrategy,
            params string[] securityParameter) :
                base(
                    securityStrategy,
                    securityParameter,
                    new HtmlSerializer())
        { }

        protected override string GetRequestId(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var querystring = GetQuerystring(uri);

            return querystring.ContainsKey(OAuth2Parameter.State.EnumToString()) ? 
                querystring[OAuth2Parameter.State.EnumToString()] : 
                null;
        }

        protected override HttpValueCollection GetQuerystring(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var query = HttpUtility.ParseQueryString(uri.Query);
            query.Add(HttpUtility.ParseQueryString(uri.Fragment));

            return query;
        }

        protected override bool IsResponseError(
            HttpValueCollection query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return query.ContainsKey(
                OAuth2Parameter.Error.EnumToString());
        }

        protected override bool IsDiscardableResponse(
            HttpValueCollection query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return query.Count == 0;
        }

        protected override bool IsIntermediateResponseValid(
            HttpValueCollection query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return query.ContainsKey(OAuth2ResponseType.Code.EnumToString()) ||
                   query.ContainsKey("accessToken") ||
                   query.ContainsKey("access_token");
        }
    }
}
