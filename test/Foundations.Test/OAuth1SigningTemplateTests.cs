using System;
using System.Net.Http;
using Foundations.Collections;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Xunit;

namespace Foundations.Test
{
    public class OAuth1SigningTemplateTests
    {
        [Fact]
        public void RequestTokenSignatureBaseGeneratesCorrectly()
        {
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var callbackUrl = new Uri("http://localhost:33533/oauth/twitter");
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";

            var httpMethod = HttpMethod.Post;
            var targetUri = new Uri("https://api.twitter.com/oauth/request_token");
            var parameters = new HttpValueCollection();

            var expected =
                "POST&https%3A%2F%2Fapi.twitter.com%2Foauth%2Frequest_token&oauth_callback%3Dhttp%253A%252F%252Flocalhost%253A33533%252Foauth%252Ftwitter%26oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_version%3D1.0";

            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                callbackUrl,
                signingAlgorithm,
                timestamp,
                nonce);

            var actual = template.ConcatenateElements(
                httpMethod, 
                targetUri, 
                parameters);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RequestTokenSignatureGeneratedCorrectlyFromSignatureBase()
        {
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var callbackUrl = new Uri("http://localhost:33533/oauth/twitter");
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";

            var signatureBase = "POST&https%3A%2F%2Fapi.twitter.com%2Foauth%2Frequest_token&oauth_callback%3Dhttp%253A%252F%252Flocalhost%253A33533%252Foauth%252Ftwitter%26oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_version%3D1.0";

            var expected = "TKRp+fb/EZatLGVyv7UILdWTb6s=";

            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                callbackUrl,
                signingAlgorithm,
                timestamp,
                nonce);

            var actual = template.GenerateSignature(signatureBase);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccessTokenSignatureBaseGeneratesCorrectly()
        {
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var verifier = "myVerifier";
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";

            var httpMethod = HttpMethod.Post;
            var targetUri = new Uri("https://api.twitter.com/oauth/request_token");
            var parameters = new HttpValueCollection();

            var expected =
                "POST&https%3A%2F%2Fapi.twitter.com%2Foauth%2Frequest_token&oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_token%3DmyOAuthToken%26oauth_verifier%3DmyVerifier%26oauth_version%3D1.0";


            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                verifier,
                signingAlgorithm,
                timestamp,
                nonce);

            var actual = template.ConcatenateElements(
                httpMethod,
                targetUri,
                parameters);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccessTokenSignatureGeneratedCorrectlyFromSignatureBase()
        {
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var verifier = "myVerifier";
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";

            var signatureBase = "POST&https%3A%2F%2Fapi.twitter.com%2Foauth%2Frequest_token&oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_token%3DmyOAuthToken%26oauth_verifier%3DmyVerifier%26oauth_version%3D1.0";

            var expected = "Z+VCg2t0dDFvbcslKCyJIEx7V9A=";

            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                verifier,
                signingAlgorithm,
                timestamp,
                nonce);

            var actual = template.GenerateSignature(signatureBase);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProtectedResourceSignatureBaseGeneratesCorrectly()
        {
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";

            var httpMethod = HttpMethod.Get;
            var targetUri = new Uri("https://api.twitter.com/oauth/verify.json");
            var parameters = new HttpValueCollection();

            var expected =
    "GET&https%3A%2F%2Fapi.twitter.com%2Foauth%2Fverify.json&oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_token%3DmyOAuthToken%26oauth_version%3D1.0";


            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                signingAlgorithm,
                timestamp,
                nonce);

            var actual = template.ConcatenateElements(
                httpMethod,
                targetUri,
                parameters);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProtectedResourceSignatureGeneratedCorrectlyFromSignatureBase()
        {
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var signingAlgorithm = DigestSigningAlgorithm.Sha1Algorithm();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";

            var signatureBase =
    "GET&https%3A%2F%2Fapi.twitter.com%2Foauth%2Fverify.json&oauth_consumer_key%3DmyConsumerKey%26oauth_nonce%3Dndhlnce3jxghrf0v%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1477075128%26oauth_token%3DmyOAuthToken%26oauth_version%3D1.0";

            var expected = "EBSM4vTvEvirRhu4Wv4jzoBE1Fo=";

            var template = new OAuth1SigningTemplate(
                consumerKey,
                consumerSecret,
                oauthToken,
                oauthSecret,
                signingAlgorithm,
                timestamp,
                nonce);

            var actual = template.GenerateSignature(signatureBase);

            Assert.Equal(expected, actual);
        }
    }
}
