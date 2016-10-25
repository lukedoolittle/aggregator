using System;
using Newtonsoft.Json.Linq;
using Material.Infrastructure.Credentials;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    
    public class TokenDeserializationTests
    {
        [Fact]
        public void DeserializeOAuth1Token()
        {
            var oauthToken = Guid.NewGuid().ToString();
            var oauthSecret = Guid.NewGuid().ToString();
            var consumerKey = Guid.NewGuid().ToString();
            var consumerSecret = Guid.NewGuid().ToString();
            var verifier = Guid.NewGuid().ToString();

            var token = new JObject
            {
                ["oauth_token"] = oauthToken,
                ["oauth_token_secret"] = oauthSecret,
                ["consumerKey"] = consumerKey,
                ["consumerSecret"] = consumerSecret,
                ["oauth_verifier"] = verifier
            };

            var serializer = new Foundations.HttpClient.Serialization.JsonSerializer();
            var actual = serializer.Deserialize<OAuth1Credentials>(token.ToString());

            Assert.Equal(oauthToken, actual.OAuthToken);
            Assert.Equal(oauthSecret, actual.OAuthSecret);
            Assert.Equal(consumerKey, actual.ConsumerKey);
            Assert.Equal(consumerSecret, actual.ConsumerSecret);
            Assert.Equal(verifier, actual.Verifier);

        }

        [Fact]
        public void DeserializeOAuth2Token()
        {
            var accessToken = Guid.NewGuid().ToString();
            var expiresIn = Guid.NewGuid().ToString();
            var tokenName = Guid.NewGuid().ToString();
            var code = Guid.NewGuid().ToString();
            var date = "2016-06-30T12:42:42.4552308-05:00";

            var token = new JObject
            {
                ["access_token"] = accessToken,
                ["expires_in"] = expiresIn,
                ["token_type"] = tokenName,
                ["dateCreated"] = date,
                ["code"] = code
            };

            var serializer = new Foundations.HttpClient.Serialization.JsonSerializer();
            var actual = serializer.Deserialize<OAuth2Credentials>(token.ToString());

            Assert.Equal(accessToken, actual.AccessToken);
            Assert.Equal(expiresIn, actual.ExpiresIn);
            Assert.Equal(tokenName, actual.TokenName);
            Assert.Equal(code, actual.Code);
            Assert.NotEqual(default(DateTimeOffset), actual.DateCreated);
        }

        [Fact]
        public void DeserializeOAuth2TokenWithAlternateParameters()
        {
            var accessToken = Guid.NewGuid().ToString();
            var expiresIn = Guid.NewGuid().ToString();
            var tokenName = Guid.NewGuid().ToString();
            var code = Guid.NewGuid().ToString();

            var token = new JObject
            {
                ["accessToken"] = accessToken,
                ["expires"] = expiresIn,
                ["token_type"] = tokenName,
                ["code"] = code
            };

            var serializer = new Foundations.HttpClient.Serialization.JsonSerializer();
            var actual = serializer.Deserialize<OAuth2Credentials>(token.ToString());

            Assert.Equal(accessToken, actual.AccessToken);
            Assert.Equal(expiresIn, actual.ExpiresIn);
            Assert.Equal(tokenName, actual.TokenName);
            Assert.Equal(code, actual.Code);
        }

        //[Fact]
        //public void DeserializeNoAuthToken()
        //{
        //    var apiKey = Guid.NewGuid().ToString();
        //    var apiKeyName = Guid.NewGuid().ToString();
        //    var serviceName = Guid.NewGuid().ToString();

        //    var token = new JObject
        //    {
        //        ["apiKey"] = apiKey,
        //        ["apiKeyName"] = apiKeyName,
        //        ["serviceName"] = serviceName
        //    };

        //    var actual = token.ToObject<NoAuthCredentials>();

        //    Assert.Equal(apiKey, actual.ApiKey);
        //    Assert.Equal(apiKeyName, actual.ApiKeyName);
        //    Assert.Equal(serviceName, actual.ServiceName);
        //}
    }
}
