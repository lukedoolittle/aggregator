using Material.Contracts;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    public class OpenIdTokenTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var clientSecret = _appRepository.GetClientSecret<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var token = await new OpenIdApp<Google>(
                        clientId,
                        redirectUri)
                    .GetCredentialsAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(TestUtilities.IsValidJsonWebToken(token));
        }
    }
}
