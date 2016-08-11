using System;
using System.Security;
using Foundations;
using Foundations.HttpClient.Serialization;
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
        private readonly ISerializer _serializer;

        protected OAuthCallbackHandlerBase(
            IOAuthSecurityStrategy securityStrategy,
            string securityParameter, 
            string userId, 
            ISerializer serializer)
        {
            _securityParameter = securityParameter;
            _userId = userId;
            _serializer = serializer;
            _securityStrategy = securityStrategy;
        }

        public virtual TToken ParseAndValidateCallback<TToken>(
            Uri responseUri)
            where TToken : TokenCredentials
        {
            var querystring = GetQuerystring(responseUri);
            if (querystring == null)
            {
                return null;
            }

            var query = HttpUtility.ParseQueryString(querystring);

            if (IsResponseError(query))
            {
                throw new OAuthCallbackErrorException();
            }

            if (!IsResponseSecure(query))
            {
                throw new SecurityException(
                    StringResources.CallbackParameterInvalidException);
            }

            var token = _serializer.Deserialize<TToken>(
                query.ToString(false));

            if (token == null || !token.AreValidIntermediateCredentials)
            {
                throw new FormatException(
                    StringResources.InvalidIntermediateToken);
            }

            return token;
        }

        protected virtual string GetQuerystring(Uri uri)
        {
            if (!string.IsNullOrEmpty(uri.Query) && uri.Query != "?")
            {
                return uri.Query;
            }

            return null;
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
