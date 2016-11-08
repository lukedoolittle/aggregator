using Material.Contracts;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    [Trait("Category", "Automated")]
    public class OAuth2JWTBearerTokenTests
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleJWTBearerGrant()
        {
            var privateKey = _appRepository.GetPrivateKey<GoogleAnalytics>();
            var clientEmail = _appRepository.GetClientEmail<GoogleAnalytics>();

            var token = await new OAuth2Assert<GoogleAnalytics>(
                privateKey,
                    clientEmail)
                .AddScope<GoogleAnalyticsReports>()
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }
    }
}
