using System;
using System.Collections.Generic;
using System.Security;
using Foundations.Collections;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Callback
{
    public abstract class OAuthCallbackHandlerBase<TCredentials> : 
        IOAuthCallbackHandler<TCredentials>
        where TCredentials : TokenCredentials
    {
        private readonly IEnumerable<string> _securityParameters;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly ISerializer _serializer;

        protected OAuthCallbackHandlerBase(
            IOAuthSecurityStrategy securityStrategy,
            IEnumerable<string> securityParameters, 
            ISerializer serializer)
        {
            _securityParameters = securityParameters;
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
            var query = GetQuerystring(responseUri);

            if (IsDiscardableResponse(query))
            {
                return null;
            }
            if (IsResponseError(query))
            {
                var errorToken = _serializer.Deserialize<TCredentials>(
                    query.ToString(false));
                errorToken.IsErrorResult = true;
                return errorToken;
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
                return null;
            }

            return token;
        }

        protected virtual HttpValueCollection GetQuerystring(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            return HttpUtility.ParseQueryString(uri.Query);
        }

        protected virtual bool IsResponseSecure(
            HttpValueCollection query,
            string userId)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            foreach (var securityParameter in _securityParameters)
            {
                string securityValue = null;
                if (query.ContainsKey(securityParameter))
                {
                    securityValue = query[securityParameter];
                }

                var isParameterValid =_securityStrategy.IsSecureParameterValid(
                    userId,
                    securityParameter,
                    securityValue);

                if (!isParameterValid)
                {
                    return false;
                }
            }

            return true;
        }

        protected abstract bool IsResponseError(
            HttpValueCollection query);

        protected abstract bool IsDiscardableResponse(
            HttpValueCollection query);
    }
}
