using System;
using Foundations.HttpClient.Enums;
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

        [Fact]
        public void CreateSignatureBaseCorrectlyCreatesSignature()
        {
            var expected = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJhbmFseXRpY3MtYXBpQG11c2ljbm90ZXMtMTQ0MjE3LmlhbS5nc2VydmljZWFjY291bnQuY29tIiwic2NvcGUiOiJodHRwczovL3d3dy5nb29nbGVhcGlzLmNvbS9hdXRoL2FuYWx5dGljcy5yZWFkb25seSIsImF1ZCI6Imh0dHBzOi8vYWNjb3VudHMuZ29vZ2xlLmNvbS9vL29hdXRoMi90b2tlbiIsImlhdCI6MTAwLCJleHAiOjIwMH0=";

            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = "analytics-api@musicnotes-144217.iam.gserviceaccount.com",
                    Scope = "https://www.googleapis.com/auth/analytics.readonly",
                    Audience = "https://accounts.google.com/o/oauth2/token",
                    ExpirationTime = 200,
                    IssuedAt = 100
                }
            };

            var actual = token.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeserializeApiKeyCredentials()
        {
            var apiKeyName = Guid.NewGuid().ToString();
            var keyValue = Guid.NewGuid().ToString();
            var keyType = HttpParameterType.Querystring;

            var token = new JObject
            {
                ["keyName"] = apiKeyName,
                ["keyValue"] = keyValue,
                ["keyType"] = (int)keyType
            };

            var serializer = new Foundations.HttpClient.Serialization.JsonSerializer();
            var actual = serializer.Deserialize<ApiKeyCredentials>(token.ToString());

            Assert.Equal(apiKeyName, actual.KeyName);
            Assert.Equal(keyValue, actual.KeyValue);
            Assert.Equal(keyType, actual.KeyType);
        }
    }
}
