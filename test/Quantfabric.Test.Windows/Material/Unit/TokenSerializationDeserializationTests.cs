using System;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Enums;
using Newtonsoft.Json.Linq;
using Material.Infrastructure.Credentials;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]

    public class TokenSerializationDeserializationTests
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

            var token = new JObject
            {
                ["access_token"] = accessToken,
                ["expires_in"] = expiresIn,
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

        [Fact]
        public void DeserializeJsonWebToken()
        {
            var audience = "exampleClientId";
            var type = "JWT";
            var algorithm = JsonWebTokenAlgorithm.RS256;
            var issuer = "https://login.quantfabric.com";
            var subject = "exampleUserId";
            var issuedAt = new DateTime(2016, 11, 21, 22, 19, 34);
            var expires = new DateTime(2016, 11, 21, 23, 19, 34);

            var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2xvZ2luLnF1YW50ZmFicmljLmNvbSIsInN1YiI6ImV4YW1wbGVVc2VySWQiLCJleHAiOjE0Nzk3NzAzNzQsImlhdCI6MTQ3OTc2Njc3NCwiYXVkIjoiZXhhbXBsZUNsaWVudElkIn0.bSGwmV_NOSsK_plB-YcuZSmwBHmBFsCBdascZItAx8s_teFSks6AknT74aM7wxatMZInEw85KrqIqyjNoDZtmMCZSt1gyeEKfQIDRemT_RJQ1zlHZqypUH1q3-azBWec_K9mkJnEF66IdTkmdWzMex2v3RPmOMcCzQskeQ4TsaWQ67GALlB8v3OrkH0hYa0h7e0sQiem208tTCcKkHuMYxhLwTNt7G_0Ujm2DQAIRmwIkiaeYmyJJQfbadlsaiFlZjRnI2XwTN0BROBpMrdgi9P3846do0XJpEVh0Y34G3Cru-epfVcpv7GKziGqToEk0M0k-O2c72vzsc5arfhf9g";

            var actualToken = token.ToWebToken();

            Assert.Equal(type, actualToken.Header.MediaType);
            Assert.Equal(algorithm, actualToken.Header.Algorithm);

            Assert.Equal(audience, actualToken.Claims.Audience);
            Assert.Equal(issuer, actualToken.Claims.Issuer);
            Assert.Equal(subject, actualToken.Claims.Subject);
            Assert.Equal(issuedAt, actualToken.Claims.IssuedAt);
            Assert.Equal(expires, actualToken.Claims.ExpirationTime);
        }
    }
}
