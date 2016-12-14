using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Material.Contracts;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.OAuth;
using Material.OAuth.Workflow;
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
                    new RsaCryptoKey(privateKey, true),
                    clientEmail)
                .AddScope<GoogleAnalyticsReports>()
                .GetCredentialsAsync(JsonWebTokenAlgorithm.RS256)
                .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidOAuth2Token(token));
        }
    }
}
