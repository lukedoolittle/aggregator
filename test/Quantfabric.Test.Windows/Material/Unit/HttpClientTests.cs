using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;
using Xunit;
using OAuth2ResponseType = Foundations.HttpClient.Enums.OAuth2ResponseType;

namespace Quantfabric.Test.Material.Unit
{
    public class HttpClientTests
    {
        private const string _endpoint = "https://httpbin.org/";
        private const string _postPath = "post";

        [Fact]
        public async void AddOAuth2JsonWebTokenAuthentication()
        {
            var expectedArgsCount = 3;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var token = new JsonWebToken(
                new JsonWebTokenHeader(), 
                new JsonWebTokenClaims());
            var clientId = Guid.NewGuid().ToString();
            var privateKey = RsaCryptoKeyPair.Create(1024).Private;

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2Assertion(
                    token,
                    privateKey,
                    new JsonWebTokenSignerFactory()));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.NotNull(response.Args["assertion"]);
            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(GrantType.JsonWebToken.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);
            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2AccessTokenAuthentication()
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

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(clientId))
                .AddParameter(new OAuth2ClientSecret(clientSecret))
                .AddParameter(new OAuth2CallbackUri(redirectUri))
                .AddParameter(new OAuth2Code(code))
                .AddParameter(new OAuth2Scope(scope));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(clientSecret, response.Args[OAuth2Parameter.ClientSecret.EnumToString()]);
            Assert.Equal(redirectUri.ToString(), response.Args[OAuth2Parameter.RedirectUri.EnumToString()]);
            Assert.Equal(code, response.Args[OAuth2ResponseType.Code.EnumToString()]);
            Assert.Equal(scope, response.Args[OAuth2Parameter.Scope.EnumToString()]);
            Assert.Equal(GrantType.AuthCode.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2ClientCredentialsAuthentication()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var clientId = Guid.NewGuid().ToString();
            var clientSecret = Guid.NewGuid().ToString();

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(clientId))
                .AddParameter(new OAuth2ClientSecret(clientSecret));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(GrantType.ClientCredentials.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);
            Assert.True(response.Headers[HttpRequestHeader.Authorization.ToString()].StartsWith("Basic"));
            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2RefreshTokenAuthentication()
        {
            var expectedArgsCount = 4;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var clientId = Guid.NewGuid().ToString();
            var clientSecret = Guid.NewGuid().ToString();
            var refreshToken = Guid.NewGuid().ToString();

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(clientId))
                .AddParameter(new OAuth2ClientSecret(clientSecret))
                .AddParameter(new OAuth2RefreshToken(refreshToken));

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(builder)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(clientSecret, response.Args[OAuth2Parameter.ClientSecret.EnumToString()]);
            Assert.Equal(refreshToken, response.Args[OAuth2Parameter.RefreshToken.EnumToString()]);
            Assert.Equal(GrantType.RefreshToken.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);

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

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(accessToken, response.Args[accessTokenName]);

            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2ProtectedResourceWithBearer()
        {
            var expectedArgsCount = 1;
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

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(accessToken, response.Args[accessTokenName]);

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

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth1ProtectedResource(
                    consumerKey,
                    consumerSecret,
                    oauthToken,
                    oauthSecret)
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
        public async void AddOAuth1RequestTokenAuthentication()
        {
            var expectedArgsCount = 7;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var consumerKey = Guid.NewGuid().ToString();
            var consumerSecret = Guid.NewGuid().ToString();
            var callbackUrl = new Uri("http://localhost:8080");

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth1RequestToken(
                    consumerKey,
                    consumerSecret,
                    callbackUrl)
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
        public async void AddOAuth1AccessTokenAuthentication()
        {
            var expectedArgsCount = 8;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var consumerKey = Guid.NewGuid().ToString();
            var consumerSecret = Guid.NewGuid().ToString();
            var oauthToken = Guid.NewGuid().ToString();
            var oauthSecret = Guid.NewGuid().ToString();
            var oauthVerifier = Guid.NewGuid().ToString();

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth1AccessToken(
                    consumerKey,
                    consumerSecret,
                    oauthToken,
                    oauthSecret,
                    oauthVerifier)
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
        public async void AddApiKeyCredentialsAuthentication()
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
