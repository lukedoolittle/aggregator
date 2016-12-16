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
    public abstract class OAuth2AuthorizationFacadeBase :
        IOAuthFacade<OAuth2Credentials>
    {
        protected IOAuthSecurityStrategy Strategy { get; }
        protected string ClientId { get; }
        protected OAuth2ResourceProvider ResourceProvider { get; }
        protected IOAuth2AuthorizationAdapter OAuth { get; }
        protected Uri CallbackUri { get; }

        protected OAuth2AuthorizationFacadeBase(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            Uri callbackUri,
            IOAuth2AuthorizationAdapter oauth,
            IOAuthSecurityStrategy strategy)
        {
            ClientId = clientId;
            ResourceProvider = resourceProvider;
            CallbackUri = callbackUri;
            OAuth = oauth;
            Strategy = strategy;
        }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public Task<Uri> GetAuthorizationUriAsync(string userId)
        {
            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(ClientId))
                .AddParameter(new OAuth2Scope(
                    ResourceProvider.Scopes,
                    ResourceProvider.ScopeDelimiter))
                .AddParameter(new OAuth2CallbackUri(CallbackUri))
                .AddParameter(new OAuth2ResponseType(
                    ResourceProvider.ResponseType))
                .AddParameters(GetSecurityParameters(userId))
                .AddParameters(ResourceProvider.Parameters.Select(
                    p => new GenericParameter(p.Key, p.Value)));

            var authorizationPath =
                OAuth.GetAuthorizationUri(
                    ResourceProvider.AuthorizationUrl,
                    builder);

            return Task.FromResult(authorizationPath);
        }

        protected virtual IList<IAuthenticatorParameter> GetSecurityParameters(
            string userId)
        {
            return new List<IAuthenticatorParameter>
            {
                new OAuth2State(Strategy, userId)
            };

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

            var accessToken = await GetRawAccessToken(
                    intermediateResult, 
                    userId)
                .ConfigureAwait(false);

            return accessToken
                .TimestampToken()
                .SetTokenName(ResourceProvider.TokenName)
                .SetClientId(ClientId);
        }

        protected abstract Task<OAuth2Credentials> GetRawAccessToken(
            OAuth2Credentials intermediateCredentials,
            string userId);
    }
}
