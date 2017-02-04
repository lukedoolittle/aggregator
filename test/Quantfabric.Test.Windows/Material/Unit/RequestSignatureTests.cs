using System;
using System.Net;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Canonicalizers;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.OAuth.AuthenticatorParameters;
using Material.OAuth.Security;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class RequestSignatureTests
    {
        [Fact]
        public void SignMicrosoftRequest()
        {
            var expected = "H6PY3Nzq4+ZWgcX+FskAOENySwp8FfhjoHY7EgbFrvA=";

            var accountName = "MyAccount";
            var accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
            var accountKeyType = "SharedKey";
            var targetUri = new Uri("https://myaccount.table.core.windows.net/sportingproducts(PartitionKey='Baseball',RowKey='BBt1032')");
            var date = new DateTime(2017, 2, 3, 21, 28, 36, DateTimeKind.Utc);

            var builder = new AuthenticatorBuilder()
                .AddParameter(new MicrosoftDate(date))
                .AddSigner(new MicrosoftRequestSigningAlgorithm(
                    accountName,
                    accountKey,
                    accountKeyType,
                    HmacDigestSigningAlgorithm.Sha256Algorithm(),
                    new MicrosoftCanonicalizer(
                        accountName)));

            var request = new HttpRequestBuilder(targetUri.NonPath())
                .GetFrom(
                    targetUri.AbsolutePath,
                    HttpParameterType.Unspecified)
                .Authenticator(builder);
            request.GenerateRequestUri();

            var actual = request.RequestHeaders[HttpRequestHeader.Authorization].Split(' ')[1].Split(':')[1];

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SignOAuth1RequestTokenRequest()
        {
            var expected = "TKRp+fb/EZatLGVyv7UILdWTb6s=";

            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var callbackUrl = new Uri("http://localhost:33533/oauth/twitter");
            var signingAlgorithm = HmacDigestSigningAlgorithm.Sha1Algorithm();
            var canonicalizer = new OAuth1Canonicalizer();
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
                    signingAlgorithm,
                    canonicalizer));

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
            var signingAlgorithm = HmacDigestSigningAlgorithm.Sha1Algorithm();
            var canonicalizer = new OAuth1Canonicalizer();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";
            var targetUri = new Uri("https://api.twitter.com/oauth/request_token");

            var userId = Guid.NewGuid().ToString();
            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

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
                .AddParameter(new OAuth1Verifier(verifier))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    consumerSecret,
                    oauthSecret,
                    signingAlgorithm,
                    canonicalizer));

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
            var expected = "EBSM4vTvEvirRhu4Wv4jzoBE1Fo=";

            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var signingAlgorithm = HmacDigestSigningAlgorithm.Sha1Algorithm();
            var canonicalizer = new OAuth1Canonicalizer();
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";
            var targetUri = new Uri("https://api.twitter.com/oauth/verify.json");

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth1ConsumerKey(consumerKey))
                .AddParameter(new OAuth1Token(oauthToken))
                .AddParameter(new OAuth1Timestamp(timestamp))
                .AddParameter(new OAuth1Nonce(nonce))
                .AddParameter(new OAuth1Version())
                .AddParameter(new OAuth1SignatureMethod(signingAlgorithm))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    consumerSecret,
                    oauthSecret,
                    signingAlgorithm,
                    canonicalizer));

            var result = new HttpRequestBuilder(targetUri.NonPath())
                .GetFrom(
                    targetUri.AbsolutePath,
                    HttpParameterType.Querystring)
                .Authenticator(builder)
                .GenerateRequestUri();

            var actual = HttpUtility.ParseQueryString(result.Query)[OAuth1Parameter.Signature.EnumToString()];

            Assert.Equal(expected, actual);
        }
    }
}
