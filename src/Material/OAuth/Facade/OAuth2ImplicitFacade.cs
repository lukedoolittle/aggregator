using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    public class OAuth2ImplicitFacade : IOAuthAccessTokenFacade<OAuth2Credentials>
    {
        private readonly string _clientId;
        private readonly OAuth2ResourceProvider _resourceProvider;

        public OAuth2ImplicitFacade(
            string clientId, 
            OAuth2ResourceProvider resourceProvider)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (resourceProvider == null) throw new ArgumentNullException(nameof(resourceProvider));

            _clientId = clientId;
            _resourceProvider = resourceProvider;
        }

        /// <summary>
        /// Returns intermediate result, which contains the access token
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Access token credentials</returns>
        public Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials intermediateResult, 
            string userId)
        {
            if (intermediateResult == null) throw new ArgumentNullException(nameof(intermediateResult));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            return Task.FromResult(intermediateResult
                .TimestampToken()
                .SetTokenName(_resourceProvider.TokenName)
                .SetClientId(_clientId));
        }
    }
}
