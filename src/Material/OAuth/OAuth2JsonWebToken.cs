using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;

namespace Material.OAuth
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2JsonWebToken<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly TResourceProvider _resourceProvider;
        private readonly string _privateKey;
        private readonly string _issuer;
        private readonly string _clientId;

        public OAuth2JsonWebToken(
            string privateKey, 
            string issuer, 
            string clientId = null,
            TResourceProvider resourceProvider = null)
        {
            _privateKey = privateKey;
            _issuer = issuer;
            _clientId = clientId;
            _resourceProvider = resourceProvider ?? new TResourceProvider();
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 Json Web Token workflow
        /// </summary>
        /// <returns>OAuth2Credentials with access token</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            var time = DateTime.Now.ToUniversalTime();
            
            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = _issuer,
                    Scope = _resourceProvider.Scope,
                    Audience = _resourceProvider.TokenUrl.ToString(),
                    ExpirationTime = Math.Floor(time.Add(TimeSpan.FromMinutes(59)).ToUnixTimeSeconds()),
                    IssuedAt = Math.Floor(time.ToUnixTimeSeconds())
                }
            };

            return new OAuthClientFacade(
                        new OAuth2AuthenticationAdapter())
                    .GetJsonWebTokenTokenCredentials(
                        token, 
                        _privateKey,
                        _clientId,
                        _resourceProvider);
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        public OAuth2JsonWebToken<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _resourceProvider.AddRequestScope<TRequest>();

            return this;
        }
    }
}
