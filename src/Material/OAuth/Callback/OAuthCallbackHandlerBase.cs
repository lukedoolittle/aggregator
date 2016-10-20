using System;
using System.Security;
using Foundations;
using Foundations.Collections;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public abstract class OAuthCallbackHandlerBase<TCredentials> : 
        IOAuthCallbackHandler<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly string _securityParameter;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly ISerializer _serializer;

        protected OAuthCallbackHandlerBase(
            IOAuthSecurityStrategy securityStrategy,
            string securityParameter, 
            ISerializer serializer)
        {
            _securityParameter = securityParameter;
            _serializer = serializer;
            _securityStrategy = securityStrategy;
        }

        /// <summary>
        /// Convert a callback uri into intermediate OAuth1Credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Intermediate OAuth1 credentials</returns>
        public virtual TCredentials ParseAndValidateCallback(
            Uri responseUri,
            string userId)
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

            if (!IsResponseSecure(query, userId))
            {
                throw new SecurityException(
                    StringResources.CallbackParameterInvalidException);
            }

            var token = _serializer.Deserialize<TCredentials>(
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

        protected virtual bool IsResponseSecure(
            HttpValueCollection query,
            string userId)
        {
            string securityValue = null;
            if (query.ContainsKey(_securityParameter))
            {
                securityValue = query[_securityParameter];
            }

            return _securityStrategy.IsSecureParameterValid(
                userId,
                _securityParameter,
                securityValue);
        }

        protected abstract bool IsResponseError(
            HttpValueCollection query);
    }
}
