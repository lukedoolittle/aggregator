using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Services;
using Aggregator.Test.Fixtures;
using Aggregator.Test.Helpers;
using Xunit;

namespace Aggregator.Test.Integration
{
    public class RefreshTokenTests : IClassFixture<OAuthRefreshTokenFixture>
    {
        private readonly OAuthRefreshTokenFixture _fixture;

        public RefreshTokenTests(OAuthRefreshTokenFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void RefreshTokenTaskForGmailGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<Google, OAuth2Credentials>();
            var newToken = await _fixture.RefreshAToken<Google>(expiredToken);
            AssertTokenDifferences(newToken, expiredToken);
        }

        [Fact]
        public async void RefreshTokenTaskForFitbitGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<Fitbit, OAuth2Credentials>();
            var newToken = await _fixture.RefreshAToken<Fitbit>(expiredToken);
            AssertTokenDifferences(newToken, expiredToken, true);
        }

        [Fact]
        public async void RefreshTokenTaskForSpotifyGivesANewToken()
        {
            var expiredToken = TestSettings.GetToken<Spotify, OAuth2Credentials>();
            var newToken = await _fixture.RefreshAToken<Spotify>(expiredToken);
            AssertTokenDifferences(newToken, expiredToken);
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
