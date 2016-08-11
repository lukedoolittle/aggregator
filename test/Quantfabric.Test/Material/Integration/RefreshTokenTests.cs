using Material;
using Aggregator.Test;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;
using Material.Infrastructure.ProtectedResources;
using Xunit;
using Fitbit = Material.Infrastructure.ProtectedResources.Fitbit;
using Google = Material.Infrastructure.ProtectedResources.Google;

namespace Quantfabric.Test.Material.Integration
{
    public class RefreshTokenTests
    {
        [Fact]
        public async void RefreshTokenTaskForGmailGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<Google, OAuth2Credentials>();
            var newToken = await new OAuth2Refresh<Google>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);
            AssertTokenDifferences(newToken, expiredToken);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Google>(newToken);
            }
        }

        [Fact]
        public async void RefreshTokenTaskForFitbitGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<Fitbit, OAuth2Credentials>();
            var newToken = await new OAuth2Refresh<Fitbit>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);
            AssertTokenDifferences(newToken, expiredToken, true);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Fitbit>(newToken);
            }
        }

        [Fact]
        public async void RefreshTokenTaskForSpotifyGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<Spotify, OAuth2Credentials>();
            var newToken = await new OAuth2Refresh<Spotify>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);
            AssertTokenDifferences(newToken, expiredToken);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<Spotify>(newToken);
            }
        }

        [Fact]
        public async void RefreshTokenTaskForTwentyThreeAndMeGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<TwentyThreeAndMe, OAuth2Credentials>();
            var newToken = await new OAuth2Refresh<TwentyThreeAndMe>()
                .RefreshCredentialsAsync(
                    expiredToken)
                .ConfigureAwait(false);
            AssertTokenDifferences(newToken, expiredToken, true);

            if (TestSettings.ShouldPersistCredentials)
            {
                TestSettings.WriteCredentials<TwentyThreeAndMe>(newToken);
            }
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
