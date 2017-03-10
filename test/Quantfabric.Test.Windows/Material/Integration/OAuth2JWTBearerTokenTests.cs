using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Material.Application;
using Material.Contracts;
using Material.Framework;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    [Trait("Category", "Automated")]
    public class OAuth2JWTBearerTokenTests
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackType.Localhost);

        public OAuth2JWTBearerTokenTests()
        {
            Platform.Current.Initialize();
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleJWTBearerGrant()
        {
            var privateKey = _appRepository.GetPrivateKey<GoogleAnalytics>();
            var clientEmail = _appRepository.GetClientEmail<GoogleAnalytics>();

            var token = await new OAuth2Assert<GoogleAnalytics>(
                    new RsaCryptoKey(
                        privateKey, 
                        true, 
                        StringEncoding.Base64),
                    clientEmail)
                .AddScope<GoogleAnalyticsReports>()
                .GetCredentialsAsync(JsonWebTokenAlgorithm.RS256)
                .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidOAuth2Token(token));
        }
    }
}
