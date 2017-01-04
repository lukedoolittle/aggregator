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

        [Fact]
        public void DeserializeJsonWebTokenForAmazonValues()
        {
            var cliendId = "amzn1.application-oa2-client.7b84d68ebec744c38d11187f17f1a5eb";
            var redirectUri = "amzn-quantfabric.material://?methodName=signin";

            var token = "eyJhbGciOiJSU0EtU0hBMjU2IiwidmVyIjoiMSJ9.eyJ2ZXIiOiIzIiwiZW5kcG9pbnRzIjp7ImF1dGh6IjoiaHR0cHM6Ly93d3cuYW1hem9uLmNvbS9hcC9vYSIsInRva2VuRXhjaGFuZ2UiOiJodHRwczovL2FwaS5hbWF6b24uY29tL2F1dGgvbzIvdG9rZW4ifSwiY2xpZW50SWQiOiJhbXpuMS5hcHBsaWNhdGlvbi1vYTItY2xpZW50LjdiODRkNjhlYmVjNzQ0YzM4ZDExMTg3ZjE3ZjFhNWViIiwiYXBwRmFtaWx5SWQiOiJhbXpuMS5hcHBsaWNhdGlvbi41MTNiNjY1ZGM5Y2M0ZmEwYjE0YTc1MzdiOTQxMzM0NSIsImJ1bmRsZVNlZWRJZCI6InF1YW50ZmFicmljLm1hdGVyaWFsIiwiYnVuZGxlSWQiOiJxdWFudGZhYnJpYy5tYXRlcmlhbCIsImlzcyI6IkFtYXpvbiIsInR5cGUiOiJBUElLZXkiLCJhcHBWYXJpYW50SWQiOiJhbXpuMS5hcHBsaWNhdGlvbi1jbGllbnQuNjg2MDBiMmVhZmIxNDY3MGI1Njc5YTc5YmJjM2FjYTIiLCJ0cnVzdFBvb2wiOm51bGwsImFwcElkIjoiYW16bjEuYXBwbGljYXRpb24tY2xpZW50LjY4NjAwYjJlYWZiMTQ2NzBiNTY3OWE3OWJiYzNhY2EyIiwiaWQiOiJlMDU5Mjg0MS1kMjk4LTExZTYtODdmZS02ZDdiNDAxNTdlYmMiLCJpYXQiOiIxNDgzNTQ2NDcyMjIyIn0=.ImdvjL460JNk7FlwytjYVE4tzcV0qD3QhL0yW9Uku6IEC+w6/sONcSutswh6qA3J9LcbYz45GkDnTBPISF1zO6yyinin5bwpbZU44CLnoQdlqvG3oNjFZVgj1ZAqqIhh0yrx95kYxKU18qKJ2ovsMm7gfz045+2ChzSaLaoC1+BxI52Dp9getzmyc2iGqSxOve3iOVYXNGo83+LIBpVR6LWDkSolNJVQP2kvSepTVkEGPkD9uHmGlCYpUkezPNelMi6CADxP626Tg8XQl/LeZfAszT5khuUj7kOydq8dOqOSICNVNuY9qyzmI8j0uQhqQZbcOcUSKTbN+CtMTpz0kA==";

            var actualToken = token.ToWebToken();

            Assert.Equal(cliendId, actualToken.Claims.ClientId);
            Assert.Equal(redirectUri, actualToken.Claims.GetAmazonCallbackUri());
        }
    }
}
