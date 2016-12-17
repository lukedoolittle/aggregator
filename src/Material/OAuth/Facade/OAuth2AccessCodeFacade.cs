using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Facade
{
    public class OAuth2AccessCodeFacade : 
        IOAuthAccessTokenFacade<OAuth2Credentials>
    {
        private readonly string _clientId;
        private readonly OAuth2ResourceProvider _resourceProvider;
        private readonly IOAuth2AuthorizationAdapter _oauth;
        private readonly Uri _callbackUri;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly IList<ISecurityParameterBundle> _securityParameters =
            new List<ISecurityParameterBundle>();
        private readonly IList<IAuthenticatorParameter> _parameters = 
            new List<IAuthenticatorParameter>();

        public OAuth2AccessCodeFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            Uri callbackUri,
            IOAuth2AuthorizationAdapter oauth,
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
        }

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
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials intermediateResult, 
            string userId)
        {
            if (intermediateResult == null) throw new ArgumentNullException(nameof(intermediateResult));

            if (intermediateResult.IsErrorResult)
            {
                return intermediateResult;
            }

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(_clientId))
                .AddParameter(new OAuth2CallbackUri(_callbackUri))
                .AddParameter(new OAuth2Code(intermediateResult.Code))
                .AddParameter(new OAuth2Scope(_resourceProvider.Scope))
                .AddParameters(_securityParameters
                    .Select(s => s.GetBundle(_securityStrategy, userId))
                    .SelectMany(s => s))
                .AddParameters(_parameters);

            var result = await _oauth.GetAccessToken(
                    _resourceProvider.TokenUrl,
                    builder,
                    _resourceProvider.Headers)
                .ConfigureAwait(false);

            return result
                .TimestampToken()
                .SetTokenName(_resourceProvider.TokenName)
                .SetClientId(_clientId);
        }
    }
}
