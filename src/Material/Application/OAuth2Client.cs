using System.Threading.Tasks;
using Material.Authorization;
using Material.Domain.Core;
using Material.Domain.Credentials;
using Material.Workflow.Facade;

namespace Material.Application
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2Client<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly OAuthClientFacade<TResourceProvider> _facade;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public OAuth2Client(
            string clientId,
            string clientSecret) : 
                this(
                    clientId,
                    clientSecret,
                    new TResourceProvider())
        { }

        public OAuth2Client(
            string clientId,
            string clientSecret,
            TResourceProvider resourceProvider)
        {
            _clientSecret = clientSecret;
            _clientId = clientId;
            _facade = new OAuthClientFacade<TResourceProvider>(
                new OAuthAuthorizationAdapter(), 
                resourceProvider);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 Client Credentials workflow
        /// </summary>
        /// <returns>OAuth2Credentials with access token</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            return _facade
                .GetClientAccessTokenCredentials(
                   _clientId, 
                   _clientSecret);
        }
    }
}