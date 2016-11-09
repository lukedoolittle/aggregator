using System;
using System.Security;
using Foundations.Collections;
using Foundations.HttpClient.Serialization;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth.Callback
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

        protected abstract bool IsDiscardableResponse(
            HttpValueCollection query);
    }
}
