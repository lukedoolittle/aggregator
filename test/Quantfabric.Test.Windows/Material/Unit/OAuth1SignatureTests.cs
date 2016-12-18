using System;
using System.Net.Http;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.OAuth.AuthenticatorParameters;
using Material.OAuth.Security;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    public class OAuth1SignatureTests
    {
        [Fact]
        public void SignOAuth1RequestTokenRequest()
        {
            var expected = "TKRp+fb/EZatLGVyv7UILdWTb6s=";

            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var callbackUrl = new Uri("http://localhost:33533/oauth/twitter");
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";
            var targetUri = new Uri("https://api.twitter.com/oauth/request_token");

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth1ConsumerKey(consumerKey))
                .AddParameter(new OAuth1Timestamp(timestamp))
                .AddParameter(new OAuth1Nonce(nonce))
                .AddParameter(new OAuth1Version())
                .AddParameter(new OAuth1SignatureMethod(signingAlgorithm))
                .AddParameter(new OAuth1CallbackUri(callbackUrl))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    consumerSecret,
                    signingAlgorithm));

            var result = new HttpRequestBuilder(targetUri.NonPath())
                .PostTo(
                    targetUri.AbsolutePath, 
                    HttpParameterType.Querystring)
                .Authenticator(builder)
                .GenerateRequestUri();

            var actual = HttpUtility.ParseQueryString(result.Query)[OAuth1Parameter.Signature.EnumToString()];

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SignOAuth1AccessTokenRequest()
        {
            var expected = "Z+VCg2t0dDFvbcslKCyJIEx7V9A=";

            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var verifier = "myVerifier";
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";
            var targetUri = new Uri("https://api.twitter.com/oauth/request_token");

            var userId = Guid.NewGuid().ToString();
            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            securityStrategy.SetSecureParameter(
                userId,
                OAuth1Parameter.Verifier.EnumToString(),
                verifier);
            securityStrategy.SetSecureParameter(
                userId, 
                OAuth1Parameter.OAuthToken.EnumToString(), 
                oauthToken);

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth1ConsumerKey(consumerKey))
                .AddParameter(new OAuth1Token(securityStrategy, userId))
                .AddParameter(new OAuth1Timestamp(timestamp))
                .AddParameter(new OAuth1Nonce(nonce))
                .AddParameter(new OAuth1Version())
                .AddParameter(new OAuth1SignatureMethod(signingAlgorithm))
                .AddParameter(new OAuth1Verifier(securityStrategy, userId))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    consumerSecret,
                    oauthSecret,
                    signingAlgorithm));

            var result = new HttpRequestBuilder(targetUri.NonPath())
                .PostTo(
                    targetUri.AbsolutePath,
                    HttpParameterType.Querystring)
                .Authenticator(builder)
                .GenerateRequestUri();

            var actual = HttpUtility.ParseQueryString(result.Query)[OAuth1Parameter.Signature.EnumToString()];

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SignOAuth1ProtectedResourceRequest()
        {
            //var signatureBase = "GET&https%3A%2F%2Fapi.twitter.com%2Foauth%2Fverify.json&oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_token%3DmyOAuthToken%26oauth_version%3D1.0";
            //var expected = "EBSM4vTvEvirRhu4Wv4jzoBE1Fo=";

            //var consumerKey = "myConsumerKey";
            //var consumerSecret = "myConsumerSecret";
            //var oauthToken = "myOAuthToken";
            //var oauthSecret = "myOAuthSecret";
            //var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            //var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            //var nonce = "ndhlnce3jxghrf0v";

            throw new NotImplementedException();
        }
    }
}
