using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    public class OAuth2JWTBearerTokenTests
    {
        private readonly AppCredentialRepository _appRepository = 
            new AppCredentialRepository(CallbackTypeEnum.Localhost);

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleJWTBearerGrant()
        {
            var privateKey = _appRepository.GetPrivateKey<GoogleAnalytics>();
            var clientEmail = _appRepository.GetClientEmail<GoogleAnalytics>();

            var token = await new OAuth2JsonWebToken<GoogleAnalytics>(privateKey, clientEmail)
                .AddScope<GoogleAnalyticsReports>()
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            Assert.True(IsValidToken(token));
        }

        private bool IsValidToken(OAuth2Credentials token)
        {
            return token != null &&
                   token.AccessToken != string.Empty &&
                   token.TokenName != string.Empty;
        }
    }
}
