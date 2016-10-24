using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Request;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;
using Material.OAuth.Facade;

namespace Material.OAuth
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2Assert<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly TResourceProvider _resourceProvider;
        private readonly OAuthClientFacade<TResourceProvider> _facade;
        private readonly string _privateKey;
        private readonly string _issuer;
        private readonly string _clientId;

        public OAuth2Assert(
            string privateKey,
            string issuer,
            string clientId = null) : 
                this(
                    privateKey,
                    issuer,
                    clientId,
                    new TResourceProvider())
        { }

        public OAuth2Assert(
            string privateKey,
            string issuer,
            string clientId,
            TResourceProvider resourceProvider)
        {
            _privateKey = privateKey;
            _issuer = issuer;
            _clientId = clientId;
            _resourceProvider = resourceProvider;
            _facade = new OAuthClientFacade<TResourceProvider>(
                new OAuth2AuthenticationAdapter(),
                resourceProvider);
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

            return _facade
                    .GetJsonWebTokenTokenCredentials(
                        token, 
                        _privateKey,
                        _clientId);
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        public OAuth2Assert<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _resourceProvider.AddRequestScope<TRequest>();

            return this;
        }
    }
}
