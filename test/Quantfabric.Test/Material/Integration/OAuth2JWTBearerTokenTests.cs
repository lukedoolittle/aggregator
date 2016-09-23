using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuth2JWTBearerTokenTests
    {
        private readonly AppCredentialRepository _appRepository;

        public OAuth2JWTBearerTokenTests()
        {
            _appRepository = new AppCredentialRepository(CallbackTypeEnum.Localhost);
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogleJWTBearerGrant()
        {
            var privateKey = _appRepository.GetPrivateKey<Google>();
            var clientEmail = _appRepository.GetClientEmail<Google>();

            var token = await new OAuth2JsonWebToken<Google>(privateKey, clientEmail)
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
