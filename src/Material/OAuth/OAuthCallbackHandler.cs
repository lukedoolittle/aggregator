using System;
using System.Security;
using Foundations;
using Foundations.Extensions;
using Foundations.Serialization;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuth2CallbackHandler : OAuthCallbackHandler
    {
        private readonly string _userId;
        private readonly IOAuthSecurityStrategy _securityStrategy;

        public OAuth2CallbackHandler(
            string userId,
            IOAuthSecurityStrategy securityStrategy)
        {
            _securityStrategy = securityStrategy;
            _userId = userId;
        }

        protected override bool IsTokenValid<TToken>(TToken token)
        {
            var baseResult = base.IsTokenValid(token);

            if (!baseResult)
            {
                return false;
            }

            var stateKey = OAuth2ParameterEnum.State.EnumToString();
            string state = null;
            if (token.AdditionalParameters.ContainsKey(stateKey))
            {
                state = token.AdditionalParameters[stateKey];
                token.AdditionalTokenParameters.Remove(stateKey);
            }

            if (!_securityStrategy.IsSecureParameterValid(_userId, stateKey, state))
            {
                throw new SecurityException(string.Format(
                    StringResources.StateParameterInvalidException,
                    state ?? string.Empty));
            }

            return true;
        }
    }

    public class OAuthCallbackHandler
    {
        public TToken ParseAndValidateCallback<TToken>(
            Uri responseUri)
            where TToken : TokenCredentials
        {
            var querystring = ParseQuerystring(responseUri);

            var token = querystring?.AsEntity<TToken>();

            return IsTokenValid(token) ? token : null;
        }

        protected HttpValueCollection ParseQuerystring(Uri uri)
        {
            string uriPart = null;

            if (!string.IsNullOrEmpty(uri.Fragment))
            {
                uriPart = uri.Fragment;
            }
            else if (!string.IsNullOrEmpty(uri.Query) && uri.Query != "?")
            {
                uriPart = uri.Query;
            }

            return uriPart != null ? 
                HttpUtility.ParseQueryString(uriPart) :
                null;
        }

        protected virtual bool IsTokenValid<TToken>(
            TToken token)
            where TToken : TokenCredentials
        {
            return token != null;
        }
    }
}
