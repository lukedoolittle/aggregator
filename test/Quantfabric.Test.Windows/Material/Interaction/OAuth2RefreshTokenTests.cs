using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    public class OAuth2RefreshTokenTests
    {
        private readonly TokenCredentialRepository _tokenRepository = 
            new TokenCredentialRepository(true);

        [Fact]
        public async void RefreshTokenTaskForGmailGivesANewToken()
        {
            var expiredToken = _tokenRepository.GetToken<Google, OAuth2Credentials>();

            var newToken = await new OAuth2Refresh<Google>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);

            AssertTokenDifferences(newToken, expiredToken);

            _tokenRepository.PutToken<Google, OAuth2Credentials>(newToken);
        }

        [Fact]
        public async void RefreshTokenTaskForFitbitGivesANewToken()
        {
            var expiredToken = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

            var newToken = await new OAuth2Refresh<Fitbit>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);

            AssertTokenDifferences(newToken, expiredToken, true);

            _tokenRepository.PutToken<Fitbit, OAuth2Credentials>(newToken);
        }

        [Fact]
        public async void RefreshTokenTaskForSpotifyGivesANewToken()
        {
            var expiredToken = _tokenRepository.GetToken<Spotify, OAuth2Credentials>();

            var newToken = await new OAuth2Refresh<Spotify>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);

            AssertTokenDifferences(newToken, expiredToken);

            _tokenRepository.PutToken<Spotify, OAuth2Credentials>(newToken);
        }

        [Fact]
        public async void RefreshTokenTaskForTwentyThreeAndMeGivesANewToken()
        {
            var expiredToken = _tokenRepository.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

            var newToken = await new OAuth2Refresh<TwentyThreeAndMe>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);

            AssertTokenDifferences(newToken, expiredToken, true);

            _tokenRepository.PutToken<TwentyThreeAndMe, OAuth2Credentials>(newToken);
        }

        [Fact]
        public async void RefreshTokenTaskForAmazonGivesANewToken()
        {
            var expiredToken = _tokenRepository.GetToken<Amazon, OAuth2Credentials>();

            var newToken = await new OAuth2Refresh<Amazon>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);

            AssertTokenDifferences(newToken, expiredToken, true);

            _tokenRepository.PutToken<Amazon, OAuth2Credentials>(newToken);
        }

        private void AssertTokenDifferences(
            OAuth2Credentials newToken, 
            OAuth2Credentials expiredToken,
            bool expectDifferentRefreshToken = false)
        {
            Assert.NotNull(newToken);
            if (!expectDifferentRefreshToken)
            {
                Assert.Equal(expiredToken.RefreshToken, newToken.RefreshToken);
            }
            Assert.True(!string.IsNullOrEmpty(newToken.AccessToken));
            Assert.NotEqual(expiredToken.AccessToken, newToken.AccessToken);
            Assert.True(!newToken.IsTokenExpired);
            Assert.False(string.IsNullOrEmpty(newToken.ClientId));
            Assert.False(string.IsNullOrEmpty(newToken.ClientSecret));
        }
    }
}
