using System;
using Material.Contracts;

namespace Material.Infrastructure.OAuth.Callback
{
    public class OAuth2FragmentCallbackHandler : 
        OAuth2QueryCallbackHandler
    {
        public OAuth2FragmentCallbackHandler(
            IOAuthSecurityStrategy securityStrategy,
            string securityParameter) :
                base(
                    securityStrategy,
                    securityParameter)
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
