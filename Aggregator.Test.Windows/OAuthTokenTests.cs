using System.IO;
using BatmansBelt.Serialization;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Serialization;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Test.Fixtures;
using Xunit;

namespace Aggregator.Test.Interaction
{
    using Aggregator.Infrastructure.Services;

    public class OAuthTokenTests : IClassFixture<OAuthTokenFixture>
    {
        private readonly OAuthTokenFixture _fixture;

        public OAuthTokenTests(OAuthTokenFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void CanGetValidAccessTokenFromTwitter()
        {
            var token = await _fixture.CreateToken<Twitter, OAuth1Credentials>();
            Assert.True(IsValidOAuth1Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromGoogle()
        {
            var token = await _fixture.CreateToken<Google, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFacebook()
        {
            var token = await _fixture.CreateToken<Facebook, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFoursquare()
        {
            var token = await _fixture.CreateToken<Foursquare, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromLinkedIn()
        {
            var token = await _fixture.CreateToken<Linkedin, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromSpotify()
        {
            var token = await _fixture.CreateToken<Spotify, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromFitbit()
        {
            var token = await _fixture.CreateToken<Fitbit, OAuth2Credentials>();
            var credentials = File.ReadAllText("../../../Aggregator.Test/testCredentials.json").AsEntity<JObject>();
            credentials["Fitbit"] = token.AsJObject();
            File.WriteAllText("../../../Aggregator.Test/testCredentials.json", credentials.AsJson());
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRunkeeper()
        {
            var token = await _fixture.CreateToken<Runkeeper, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromRescuetime()
        {
            var token = await _fixture.CreateToken<Rescuetime, OAuth2Credentials>();
            Assert.True(IsValidOAuth2Token(token));
        }

        [Fact(Skip = "Restsharp bug breaks this integration (issue 810 on github")]
        public async void CanGetValidAccessTokenFromFatsecret()
        {
            var token = await _fixture.CreateToken<Fatsecret, OAuth1Credentials>();
            Assert.True(IsValidOAuth1Token(token));
        }

        [Fact]
        public async void CanGetValidAccessTokenFromWithings()
        {
            var token = await _fixture.CreateToken<Withings, OAuth1Credentials>();
            Assert.True(IsValidOAuth1Token(token));
        }

        private bool IsValidOAuth1Token(OAuth1Credentials token)
        {
            return token != null &&
                   token.ConsumerKey != string.Empty &&
                   token.ConsumerSecret != string.Empty &&
                   token.OAuthToken != string.Empty &&
                   token.OAuthSecret != string.Empty;
        }

        private bool IsValidOAuth2Token(OAuth2Credentials token)
        {
            return token != null &&
                   token.AccessToken != string.Empty &&
                   token.TokenName != string.Empty;
        }
    }
}
