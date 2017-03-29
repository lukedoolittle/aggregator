using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.HttpClient.Authenticators;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Workflow.Facade
{
    public class OAuth2AccessCodeFacade : 
        IOAuthAccessTokenFacade<OAuth2Credentials>
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly OAuth2ResourceProvider _resourceProvider;
        private readonly IOAuthAuthorizationAdapter _oauth;
        private readonly Uri _callbackUri;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly IList<ISecurityParameterBundle> _securityParameters =
            new List<ISecurityParameterBundle>();
        private readonly IList<IAuthenticatorParameter> _parameters = 
            new List<IAuthenticatorParameter>();

        public OAuth2AccessCodeFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string clientSecret,
            Uri callbackUri,
            IOAuthAuthorizationAdapter oauth,
            IOAuthSecurityStrategy securityStrategy)
        {
            if (resourceProvider == null) throw new ArgumentNullException(nameof(resourceProvider));
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (callbackUri == null) throw new ArgumentNullException(nameof(callbackUri));
            if (oauth == null) throw new ArgumentNullException(nameof(oauth));
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));

            _clientId = clientId;
            _resourceProvider = resourceProvider;
            _callbackUri = callbackUri;
            _oauth = oauth;
            _securityStrategy = securityStrategy;
            _clientSecret = clientSecret;

            _resourceProvider.SetGrant(GrantType.AuthCode);
        }

        public OAuth2AccessCodeFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            Uri callbackUri,
            IOAuthAuthorizationAdapter oauth,
            IOAuthSecurityStrategy securityStrategy) : 
                this(
                    resourceProvider, 
                    clientId, 
                    null, 
                    callbackUri,
                    oauth,
                    securityStrategy)
        { }

        public OAuth2AccessCodeFacade AddParameters(
            params IAuthenticatorParameter[] authenticationParameters)
        {
            if (authenticationParameters == null) throw new ArgumentNullException(nameof(authenticationParameters));

            foreach (var parameter in authenticationParameters)
            {
                _parameters.Add(parameter);
            }

            return this;
        }

        public OAuth2AccessCodeFacade AddSecurityParameters(
            params ISecurityParameterBundle[] securityParameters)
        {
            if (securityParameters == null) throw new ArgumentNullException(nameof(securityParameters));

            foreach (var securityParameter in securityParameters)
            {
                _securityParameters.Add(securityParameter);
            }

            return this;
        }

        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="requestId">Unique ID for the request</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials intermediateResult, 
            string requestId)
        {
            if (intermediateResult == null) throw new ArgumentNullException(nameof(intermediateResult));
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            if (intermediateResult.IsErrorResult)
            {
                return intermediateResult;
            }

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(_clientId))
                .AddParameter(new OAuth2Callback(_callbackUri))
                .AddParameter(new OAuth2Code(intermediateResult.Code))
                .AddParameter(new OAuth2Scope(_resourceProvider.Scope))
                .AddParameter(new OAuth2GrantType(_resourceProvider.Grant))
                .AddParameters(_securityParameters
                    .Select(s => s.GetBundle(_securityStrategy, requestId))
                    .SelectMany(s => s))
                .AddParameters(_parameters);

            if (_clientSecret != null)
            {
                builder.AddParameter(new OAuth2ClientSecret(_clientSecret));
            }

            var result = await _oauth.GetToken<OAuth2Credentials>(
                    _resourceProvider.TokenUrl,
                    builder,
                    _resourceProvider.Headers,
                    HttpParameterType.Unspecified)
                .ConfigureAwait(false);

            return result
                .TimestampToken()
                .SetTokenName(_resourceProvider.TokenName)
                .SetClientProperties(
                    _clientId, 
                    _clientSecret);
        }
    }
}
