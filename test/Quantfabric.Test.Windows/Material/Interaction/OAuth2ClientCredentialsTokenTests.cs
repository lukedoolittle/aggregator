using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.OAuth;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    [Trait("Category", "Automated")]
    public class OAuth2ClientCredentialsTokenTests
    {

        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);
        private readonly TokenCredentialRepository _tokenRepository = 
            new TokenCredentialRepository(true);

        [Fact]
        public async void CanGetValidAccessTokenFromOmnitureClientCredentialsGrant()
        {
            var clientId = _appRepository.GetClientId<Omniture>();
            var clientSecret = _appRepository.GetClientSecret<Omniture>();

            var token = await new OAuth2Client<Omniture>(
                clientId,
                    clientSecret)
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));

            _tokenRepository.PutToken<Omniture, OAuth2Credentials>(token);
        }
    }
}
