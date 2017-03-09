using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Canonicalizers;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;
using Material.OAuth.Security;
using Xunit;
using OAuth2ResponseType = Foundations.HttpClient.Enums.OAuth2ResponseType;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class HttpClientTests
    {
        private const string _endpoint = "https://httpbin.org/";
        private const string _postPath = "post";
        private const string _getPath = "get";

        [Fact]
        public async void AddMicrosoftAuthorization()
        {
            var expected = "2Be4oE1RkO0u5ie8mJfbaYHMzWl7uSMpsFrkRgdseXY=";

            var accountName = "MyAccount";
            var accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
            var accountKeyType = "SharedKey";
            var targetUri = new Uri($"{_endpoint}{_getPath}");
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

            var response = await new HttpRequestBuilder(targetUri.NonPath())
                .GetFrom(
                    targetUri.AbsolutePath,
                    HttpParameterType.Unspecified)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            var actual = response.Headers[HttpRequestHeader.Authorization.ToString()].Split(' ')[1].Split(':')[1];

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void AddOAuth2JsonWebTokenAuthorization()
        {
            var expectedArgsCount = 3;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var token = new JsonWebToken(
                new JsonWebTokenHeader
                {
                    Algorithm = JsonWebTokenAlgorithm.RS256
                }, 
                new JsonWebTokenClaims());
            var clientId = Guid.NewGuid().ToString();
            var privateKey = RsaCryptoKeyPair.Create(1024).Private;
            var grantType = GrantType.JsonWebToken;

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(clientId))
                .AddParameter(new OAuth2Assertion(
                    token,
                    privateKey,
                    new JsonWebTokenSignerFactory()))
                .AddParameter(new OAuth2GrantType(grantType));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.NotNull(response.Args["assertion"]);
            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(grantType.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);
            Assert.Equal(expected.SomeKey, response.Json.SomeKey);

            Assert.Equal(expectedArgsCount, response.Args.Count);
        }

        [Fact]
        public async void AddOAuth2AccessTokenAuthorization()
        {
            var expectedArgsCount = 6;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var clientId = Guid.NewGuid().ToString();
            var clientSecret = Guid.NewGuid().ToString();
            var redirectUri = new Uri("http://localhost:8080");
            var code = Guid.NewGuid().ToString();
            var scope = Guid.NewGuid().ToString();
            var grantType = GrantType.AuthCode;

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(clientId))
                .AddParameter(new OAuth2ClientSecret(clientSecret))
                .AddParameter(new OAuth2Callback(redirectUri))
                .AddParameter(new OAuth2Code(code))
                .AddParameter(new OAuth2Scope(scope))
                .AddParameter(new OAuth2GrantType(grantType));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);
            
            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(clientSecret, response.Args[OAuth2Parameter.ClientSecret.EnumToString()]);
            Assert.Equal(redirectUri.ToString(), response.Args[OAuth2Parameter.RedirectUri.EnumToString()]);
            Assert.Equal(code, response.Args[OAuth2ResponseType.Code.EnumToString()]);
            Assert.Equal(scope, response.Args[OAuth2Parameter.Scope.EnumToString()]);
            Assert.Equal(grantType.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);

            Assert.Equal(expectedArgsCount, response.Args.Count);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2ClientCredentialsAuthorization()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var clientId = Guid.NewGuid().ToString();
            var clientSecret = Guid.NewGuid().ToString();
            var grantType = GrantType.ClientCredentials;

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientCredentials(
                    clientId, 
                    clientSecret))
                .AddParameter(new OAuth2GrantType(grantType));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(grantType.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);
            Assert.True(response.Headers[HttpRequestHeader.Authorization.ToString()].StartsWith("Basic"));
            Assert.Equal(expected.SomeKey, response.Json.SomeKey);

            Assert.Equal(expectedArgsCount, response.Args.Count);
        }

        [Fact]
        public async void AddOAuth2RefreshTokenAuthorization()
        {
            var expectedArgsCount = 4;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var clientId = Guid.NewGuid().ToString();
            var clientSecret = Guid.NewGuid().ToString();
            var refreshToken = Guid.NewGuid().ToString();
            var grantType = GrantType.RefreshToken;

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(clientId))
                .AddParameter(new OAuth2ClientSecret(clientSecret))
                .AddParameter(new OAuth2RefreshToken(refreshToken))
                .AddParameter(new OAuth2GrantType(grantType));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(clientSecret, response.Args[OAuth2Parameter.ClientSecret.EnumToString()]);
            Assert.Equal(refreshToken, response.Args[OAuth2Parameter.RefreshToken.EnumToString()]);
            Assert.Equal(grantType.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);

            Assert.Equal(expectedArgsCount, response.Args.Count);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2ProtectedResource()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var accessToken = Guid.NewGuid().ToString();
            var accessTokenName = "accessToken";

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2AccessToken(
                    accessToken, 
                    accessTokenName));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(accessToken, response.Args[accessTokenName]);

            Assert.Equal(expectedArgsCount, response.Args.Count);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2ProtectedResourceWithBearer()
        {
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var accessToken = Guid.NewGuid().ToString();
            var accessTokenName = "Bearer";

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2AccessToken(
                    accessToken,
                    accessTokenName));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.True(response.Headers[HttpRequestHeader.Authorization.ToString()].StartsWith("Bearer " + accessToken));

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth1ProtectedResource()
        {
            var expectedArgsCount = 7;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var consumerKey = Guid.NewGuid().ToString();
            var consumerSecret = Guid.NewGuid().ToString();
            var oauthToken = Guid.NewGuid().ToString();
            var oauthSecret = Guid.NewGuid().ToString();
            var signingAlgorithm = HmacDigestSigningAlgorithm.Sha1Algorithm();
            var canonicalizer = new OAuth1Canonicalizer();
            var stringGenerator = new CryptoStringGenerator();

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth1ConsumerKey(consumerKey))
                .AddParameter(new OAuth1Token(oauthToken))
                .AddParameter(new OAuth1Timestamp())
                .AddParameter(new OAuth1Nonce(stringGenerator))
                .AddParameter(new OAuth1Version())
                .AddParameter(new OAuth1SignatureMethod(signingAlgorithm))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    consumerSecret,
                    oauthSecret,
                    signingAlgorithm,
                    canonicalizer));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.NotNull(response.Args[OAuth1Parameter.OAuthToken.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.ConsumerKey.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Timestamp.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Nonce.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Version.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.SignatureMethod.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Signature.EnumToString()]);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth1RequestTokenAuthorization()
        {
            var expectedArgsCount = 7;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var consumerKey = Guid.NewGuid().ToString();
            var consumerSecret = Guid.NewGuid().ToString();
            var callbackUrl = new Uri("http://localhost:8080");
            var signingAlgorithm = HmacDigestSigningAlgorithm.Sha1Algorithm();
            var canonicalizer = new OAuth1Canonicalizer();
            var stringGenerator = new CryptoStringGenerator();

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth1ConsumerKey(consumerKey))
                .AddParameter(new OAuth1Timestamp())
                .AddParameter(new OAuth1Nonce(stringGenerator))
                .AddParameter(new OAuth1Version())
                .AddParameter(new OAuth1SignatureMethod(signingAlgorithm))
                .AddParameter(new OAuth1Callback(callbackUrl))
                .AddSigner(new OAuth1RequestSigningAlgorithm(
                    consumerSecret,
                    signingAlgorithm,
                    canonicalizer));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.NotNull(response.Args[OAuth1Parameter.ConsumerKey.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Callback.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Timestamp.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Nonce.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Version.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.SignatureMethod.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Signature.EnumToString()]);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth1AccessTokenAuthorization()
        {
            var expectedArgsCount = 8;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var consumerKey = "myConsumerKey";
            var consumerSecret = "myConsumerSecret";
            var oauthToken = "myOAuthToken";
            var oauthSecret = "myOAuthSecret";
            var verifier = "myVerifier";
            var timestamp = new DateTime(2016, 10, 21, 18, 38, 48, DateTimeKind.Utc);
            var nonce = "ndhlnce3jxghrf0v";
            var signingAlgorithm = HmacDigestSigningAlgorithm.Sha1Algorithm();
            var canonicalizer = new OAuth1Canonicalizer();

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

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.NotNull(response.Args[OAuth1Parameter.OAuthToken.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.ConsumerKey.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Timestamp.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Nonce.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Version.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.SignatureMethod.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Signature.EnumToString()]);
            Assert.NotNull(response.Args[OAuth1Parameter.Verifier.EnumToString()]);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddApiKeyCredentialsAuthorization()
        {
            var expectedArgsCount = 0;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var keyname = "Some-Key-Name";
            var keyvalue = Guid.NewGuid().ToString();
            var keyType = HttpParameterType.Header;

            var builder = new AuthenticatorBuilder()
                .AddParameter(
                    new ApiKey(
                        keyname, 
                        keyvalue, 
                        keyType));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(response.Headers[keyname], keyvalue);
            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }
    }

    [DataContract]
    public class SampleBody
    {
        [DataMember(Name = "somekey")]
        public string SomeKey { get; set; }
    }

    [DataContract]
    public class HttpBinResponse
    {
        [DataMember(Name = "gzipped")]
        public bool IsGZipped { get; set; }

        [DataMember(Name = "deflated")]
        public bool IsDeflated { get; set; }

        [DataMember(Name = "args")]
        public IDictionary<string, string> Args { get; set; }

        [DataMember(Name = "data")]
        public string Data { get; set; }

        [DataMember(Name = "headers")]
        public Dictionary<string, string> Headers { get; set; }

        [DataMember(Name = "form")]
        public Dictionary<string, string> Form { get; set; }

        [DataMember(Name = "origin")]
        public string Origin { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }

    [DataContract]
    public class TypedHttpBinResponse<T> : HttpBinResponse
    {
        [DataMember(Name = "json")]
        public T Json { get; set; }
    }
}
