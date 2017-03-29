using Material.Application;
using Material.Contracts;
using Material.Framework;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Interactive.Test.Windows
{
    [Trait("Category", "Manual")]
    public class OpenIdTokenTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackType.Localhost);

        public OpenIdTokenTests()
        {
            Platform.Current.Initialize();
        }

        [Fact]
        public async void CanGetValidOpenIdTokenFromGoogleCodeFlowTwice()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var clientSecret = _appRepository.GetClientSecret<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var app = new OpenIdApp<Google>(
                        clientId,
                        redirectUri);
            var token = await app.GetWebTokenAsync(clientSecret).ConfigureAwait(false);
            token = await app.GetWebTokenAsync(clientSecret).ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidJsonWebToken(token));
        }

        [Fact]
        public async void CanGetValidOpenIdTokenFromGoogleCodeFlow()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var clientSecret = _appRepository.GetClientSecret<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var token = await new OpenIdApp<Google>(
                        clientId,
                        redirectUri)
                    .GetWebTokenAsync(clientSecret)
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidJsonWebToken(token));
        }

        [Fact]
        public async void CanGetValidOpenIdTokenFromGoogleImplicitFlow()
        {
            var clientId = _appRepository.GetClientId<Google>();
            var redirectUri = _appRepository.GetRedirectUri<Google>();

            var token = await new OpenIdApp<Google>(
                        clientId,
                        redirectUri)
                    .GetWebTokenAsync()
                    .ConfigureAwait(false);

            Assert.True(ValidationUtilities.IsValidJsonWebToken(token));
        }
    }
}
