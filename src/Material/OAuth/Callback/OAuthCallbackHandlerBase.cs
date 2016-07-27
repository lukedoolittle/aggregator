using System;
using System.Security;
using Foundations;
using Foundations.Serialization;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public abstract class OAuthCallbackHandlerBase : IOAuthCallbackHandler
    {
        private readonly string _securityParameter;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;

        protected OAuthCallbackHandlerBase(
            IOAuthSecurityStrategy securityStrategy,
            string securityParameter, 
            string userId)
        {
            _securityParameter = securityParameter;
            _userId = userId;
            _securityStrategy = securityStrategy;
        }

        public TToken ParseAndValidateCallback<TToken>(
            Uri responseUri)
            where TToken : TokenCredentials
        {
            var query = ParseQuerystring(responseUri);

            if (IsResponseError(query))
            {
                throw new OAuthCallbackErrorException();
            }

            if (!IsResponseSecure(query))
            {
                throw new SecurityException(
                    StringResources.CallbackParameterInvalidException);
            }

            var token = query?.AsEntity<TToken>();

            if (token == null || !token.AreValidIntermediateCredentials)
            {
                throw new FormatException(
                    StringResources.InvalidIntermediateToken);
            }

            return token;
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

        protected virtual bool IsResponseSecure(HttpValueCollection query)
        {
            string securityValue = null;
            if (query.ContainsKey(_securityParameter))
            {
                securityValue = query[_securityParameter];
            }

            return _securityStrategy.IsSecureParameterValid(
                _userId,
                _securityParameter,
                securityValue);
        }

        protected abstract bool IsResponseError(
            HttpValueCollection query);
    }
}
