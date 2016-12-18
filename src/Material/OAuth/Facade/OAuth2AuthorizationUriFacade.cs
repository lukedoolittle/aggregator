using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.Infrastructure;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Facade
{
    public class OAuth2AuthorizationUriFacade : 
        IOAuthAuthorizationUriFacade
    {
        private readonly string _clientId;
        private readonly OAuth2ResourceProvider _resourceProvider;
        private readonly IOAuthAuthorizationAdapter _oauth;
        private readonly Uri _callbackUri;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly IList<ISecurityParameterBundle> _securityParameters = 
            new List<ISecurityParameterBundle>();

        public OAuth2AuthorizationUriFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
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
        }

        public OAuth2AuthorizationUriFacade AddSecurityParameters(
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
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public Task<Uri> GetAuthorizationUriAsync(string userId)
        {
            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(_clientId))
                .AddParameter(new OAuth2Scope(
                    _resourceProvider.Scopes,
                    _resourceProvider.ScopeDelimiter))
                .AddParameter(new OAuth2CallbackUri(_callbackUri))
                .AddParameter(new OAuth2ResponseType(
                    _resourceProvider.ResponseType))
                .AddParameters(_securityParameters
                    .Select(s => s.GetBundle(_securityStrategy, userId))
                    .SelectMany(s => s))
                .AddParameters(_resourceProvider.Parameters.Select(
                    p => new GenericParameter(p.Key, p.Value)));

            var authorizationPath =
                _oauth.GetAuthorizationUri(
                    _resourceProvider.AuthorizationUrl,
                    builder);

            return Task.FromResult(authorizationPath);
        }
    }
}
