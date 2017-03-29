using System;
using Material.Domain.Credentials;
using Material.Contracts;
using Material.Framework.Collections;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.Framework.Serialization;

namespace Material.Workflow.Callback
{
    public class OAuth1CallbackHandler : 
        OAuthCallbackHandlerBase<OAuth1Credentials>
    {
        public OAuth1CallbackHandler(
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

            return uri.GetEndOfPath();
        }

        protected override HttpValueCollection GetQuerystring(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            return HttpUtility.ParseQueryString(uri.Query);
        }

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

        protected override bool IsIntermediateResponseValid(
            HttpValueCollection query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return query.ContainsKey(
                OAuth1Parameter.Verifier.EnumToString());
        }
    }
}
