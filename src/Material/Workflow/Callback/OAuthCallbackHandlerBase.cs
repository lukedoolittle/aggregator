using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Security;
using Material.Contracts;
using Material.Framework.Collections;
using Material.Framework.Serialization;

namespace Material.Workflow.Callback
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
        /// Convert a callback uri into intermediate credentials with metadata
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Intermediate OAuth1 credentials</returns>
        public virtual CredentialMetadata<TCredentials> ParseAndValidateCallback(
            Uri responseUri)
        {
            var query = GetQuerystring(responseUri);
            
            TCredentials credentials = null;
            var requestId = GetRequestId(responseUri);
            var isResponseError = false;
            var areValidIntermediateCredentials = true;

            if (IsDiscardableResponse(query))
            {
                areValidIntermediateCredentials = false;
            }
            else if (IsResponseError(query))
            {
                credentials = _serializer.Deserialize<TCredentials>(
                    query.ToString(false));
                isResponseError = true;
            }
            else if (requestId == null || !IsResponseSecure(query, requestId))
            {
                throw new SecurityException(
                    StringResources.CallbackParameterInvalidException);
            }
            else if (!IsIntermediateResponseValid(query))
            {
                credentials = _serializer.Deserialize<TCredentials>(
                    query.ToString(false));
                areValidIntermediateCredentials = false;
            }
            else
            {
                credentials = _serializer.Deserialize<TCredentials>(
                    query.ToString(false));
            }

            return new CredentialMetadata<TCredentials>(
                credentials,
                requestId,
                areValidIntermediateCredentials,
                isResponseError);
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

        protected abstract bool IsIntermediateResponseValid(
            HttpValueCollection query);

        protected abstract HttpValueCollection GetQuerystring(
            Uri uri);

        protected abstract string GetRequestId(Uri uri);
    }
}
