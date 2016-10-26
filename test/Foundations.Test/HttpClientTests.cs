using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using Foundations.Enums;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Extensions;
using Foundations.HttpClient.Request;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Foundations.Test
{
    public class HttpClientTests
    {
        private const string _endpoint = "https://httpbin.org/";
        private const string _getPath = "get";
        private const string _postPath = "post";
        private const string _gzipPath = "gzip";
        private const string _deflatePath = "deflate";
        private const string _statusPath = "status/{code}";
        private const string _cookiePath = "cookies/set";

        #region Get Requests

        [Fact]
        public async void MakeBasicHttpGetRequest()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_getPath)
                .ResultAsync()
                .ConfigureAwait(false);

            var actual = JObject.Parse(response);

            Assert.NotNull(actual["url"]?.ToString());
        }

        [Fact]
        public async void MakeBasicHttpGetRequestForceBody()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_getPath, HttpParameterType.Body)
                .ResultAsync()
                .ConfigureAwait(false);

            var actual = JObject.Parse(response);

            Assert.NotNull(actual["url"]?.ToString());
        }

        [Fact]
        public async void MakeHttpGetRequestWithNoContentAndParameters()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_getPath)
                .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Null(response.Json);
            Assert.Null(response.Data);
            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(expected.SomeKey, response.Args[nameof(expected.SomeKey)]);
        }

        [Fact]
        public async void MakeHttpGetRequestWithContentAndNoParametersExpectException()
        {
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            await Assert.ThrowsAsync<NotSupportedException>(async () => 
                    await new HttpRequestBuilder(_endpoint)
                        .GetFrom(_getPath)
                        .Content(expected, MediaType.Json)
                        .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [Fact]
        public async void MakeHttpGetRequestWithContentAndParametersExpectException()
        {
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            await Assert.ThrowsAsync<NotSupportedException>(async () =>
                    await new HttpRequestBuilder(_endpoint)
                        .GetFrom(_getPath)
                        .Content(expected, MediaType.Json)
                        .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                        .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [Fact]
        public async void MakeHttpGetRequestWithContentAndParametersForceBodyExpectException()
        {
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            await Assert.ThrowsAsync<NotSupportedException>(async () =>
                    await new HttpRequestBuilder(_endpoint)
                        .GetFrom(_getPath, HttpParameterType.Body)
                        .Content(expected, MediaType.Json)
                        .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                        .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [Fact]
        public async void MakeHttpGetRequestWithNoContentAndParametersForceBodyExpectException()
        {
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            await Assert.ThrowsAsync<NotSupportedException>(async () =>
                    await new HttpRequestBuilder(_endpoint)
                        .GetFrom(_getPath, HttpParameterType.Body)
                        .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                        .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [Fact]
        public async void MakeHttpGetRequestWithContentAndNoParametersForceBodyExpectException()
        {
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            await Assert.ThrowsAsync<NotSupportedException>(async () =>
                    await new HttpRequestBuilder(_endpoint)
                        .GetFrom(_getPath, HttpParameterType.Body)
                        .Content(expected, MediaType.Json)
                        .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        #endregion Get Requests

        #region Post Requests

        [Fact]
        public async void MakeBasicHttpPostRequest()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .ResultAsync()
                .ConfigureAwait(false);

            var actual = JObject.Parse(response);

            Assert.NotNull(actual["url"]?.ToString());
        }

        [Fact]
        public async void MakeBasicHttpPostRequestForceQuerystring()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .ResultAsync()
                .ConfigureAwait(false);

            var actual = JObject.Parse(response);

            Assert.NotNull(actual["url"]?.ToString());
        }

        [Fact]
        public async void MakePostRequestWithContentAndNoParameters()
        {
            var expectedArgsCount = 0;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .Content(expected, MediaType.Json)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            var actual = response.Json;

            Assert.Equal(expected.SomeKey, actual.SomeKey);
            Assert.Equal(expectedArgsCount, response.Args.Count);
        }

        [Fact]
        public async void MakePostRequestWithNoContentAndParameters()
        {
            var expectedArgsCount = 0;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expected.SomeKey, response.Form[nameof(expected.SomeKey)]);
            Assert.Empty(response.Data);
            Assert.Null(response.Json);
            Assert.Equal(expectedArgsCount, response.Args.Count);
        }

        [Fact]
        public async void MakePostRequestWithContentAndParameters()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .Content(expected, MediaType.Json)
                .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            var actual = response.Json;

            Assert.Equal(expected.SomeKey, actual.SomeKey);
            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(expected.SomeKey, response.Args[nameof(expected.SomeKey)]);
        }

        [Fact]
        public async void MakePostRequestWithContentAndParametersForcingQuerystring()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath, HttpParameterType.Querystring)
                .Content(expected, MediaType.Json)
                .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            var actual = response.Json;

            Assert.Equal(expected.SomeKey, actual.SomeKey);
            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(expected.SomeKey, response.Args[nameof(expected.SomeKey)]);
        }

        [Fact]
        public async void MakePostRequestWithContentAndNoParametersForcingQuerystring()
        {
            var expectedArgsCount = 0;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath, HttpParameterType.Querystring)
                .Content(expected, MediaType.Json)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            var actual = response.Json;

            Assert.Equal(expected.SomeKey, actual.SomeKey);
            Assert.Equal(expectedArgsCount, response.Args.Count);
        }

        [Fact]
        public async void MakePostRequestWithNoContentAndParametersForcingQuerystring()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath, HttpParameterType.Querystring)
                .Parameter(nameof(expected.SomeKey), expected.SomeKey)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Null(response.Json);
            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Empty(response.Data);
            Assert.Equal(expected.SomeKey, response.Args[nameof(expected.SomeKey)]);
        }

        #endregion Post Requests

        [Fact]
        public async void MakePostRequestWithStreamAsContent()
        {
            var stream = File.OpenRead("brian.wav");
            var contentType = MediaType.Wave;

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .StreamingContent(stream, contentType)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.NotNull(response.Data);
            Assert.Equal(contentType.EnumToString(), response.Headers["Content-Type"]);
        }

        [Fact]
        public async void MakePostRequestWithRawBytesAsContent()
        {
            var rawBytes = System.IO.File.ReadAllBytes("brian.wav");
            var contentType = MediaType.Wave;

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .RawContent(rawBytes, contentType)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.NotNull(response.Data);
            Assert.Equal(contentType.EnumToString(), response.Headers["Content-Type"]);
        }

        [Fact]
        public async void MakeRequestWithUrlSegments()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom("{path}")
                .Segment("path", _getPath)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(_endpoint + _getPath, response.Url);
        }

        public async void MakeRequestForGZipEncodedData()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_gzipPath)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.True(response.IsGZipped);
        }

        public async void MakeRequestForDeflateEncodedData()
        {
            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_deflatePath)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.True(response.IsDeflated);
        }

        [Fact]
        public async void DoesNotGetAnHttpOkCodeThrowsException()
        {
            var expected = HttpStatusCode.OK;
            var actual = HttpStatusCode.NotFound;

            await Assert.ThrowsAsync<HttpRequestException>(async () => 
                    await new HttpRequestBuilder(_endpoint)
                        .GetFrom(_statusPath)
                        .Segment("code", ((int) actual).ToString())
                        .ThrowIfNotExpectedResponseCode(expected)
                        .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
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
        public async void AddOAuth2ProtectedResource()
        {
            var expectedArgsCount = 1;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var accessToken = Guid.NewGuid().ToString();
            var accessTokenName = "accessToken";

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth2ProtectedResource(
                    accessToken, 
                    accessTokenName)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(accessToken, response.Args[accessTokenName]);

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

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth2AccessToken(
                    clientId, 
                    clientSecret, 
                    redirectUri, 
                    code, 
                    scope)
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

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth2ClientAccessToken(
                    clientId,
                    clientSecret)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.Equal(GrantType.ClientCredentials.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);
            Assert.True(response.Headers[HttpRequestHeader.Authorization.ToString()].StartsWith("Basic"));
            Assert.Equal(expected.SomeKey, response.Json.SomeKey);
        }

        [Fact]
        public async void AddOAuth2JsonWebTokenAuthentication()
        {
            var expectedArgsCount = 3;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var jwt = new JsonWebToken();
            var clientId = Guid.NewGuid().ToString();
            var privateKey = @"-----BEGIN PRIVATE KEY-----
MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDdIEipITNraQrf
bVwmwOayuLSPMlIlPAgddt3pmLSdxymfF1VzqYljNCpv+ryPIoOsZcGm4/+TmNXu
+j/vTWNv9TMUpD8aJUM70GA1d5sRDuNRn2AiDRLVfbaF2fYk/zpOsKGDxbH3Wqe2
5zOleCxJTH/pKTDCF7/fQ+c2Gp06wtQB553YNrtQlIxmjkCuleZxSokAVecCSYKX
tnNSISS9c7G0ogXYXr/QAg3L1BAgXH8oOYaq1utCsNvT7hX42svJdJwnCjBZ9AUv
6MWFvLjwbBidxoWfXmM1KiHv8ZfN3I/z8bQgfj01qHEpWqv9C9jL3w2hPnP6GHrW
ai8Tl3gxAgMBAAECggEAIbhsnC4N81E/cTbyGI7OH27/Sd74m+j9q9CWoqrA0Faw
yCv8wfiWlOQ9nHn2CzXOMpoJ3/Ng5BcoeJr86Pc6NLaFpZ4uaURJbnOmWED3CrDk
hWvycv7fYmMbVGoamSW6tIlG+BtLula+wKudOpyK1FqwHtRDNTX98oQeXCfO1kjb
C4iz9LrGqLbXByDwgBI93/hCptznLcIuuO5tryKEMCZodiZbHtpu9XTJw5XeSbhf
24kW+ZYkbINo/Q3tOPGc2iTwIYPlA8/8fCjs13tkldqxYsh3AIwAmxrG0haJOCJK
KUf/HWQ2Ypk4EV0jDJlcxxYBTBtPSvXjw7OCWy7OiQKBgQDyjaLLkIdw9CKtZDLR
pASqL+RPij1XHDxScPpzx3qaN0/iin77AsV0LXuhJIDHxIVP816leHQ50bqCeRrF
OKOIuFG1QHZKnOw9fgycCxwuCqhiUHym6H72xr4p5TV0P6ApJN4UJbFa1VzCDM7F
vq/zenhxPsxo+fR1Lb0zxi/H+wKBgQDpYoyo84uMDU5f9TETT/dJ04GDt5SKF7Xg
64iL+esoCpt39KGMuXwMKa2iy4DutysCOmo10AuMpAspVvWvJZSB1LB/JZdD21VV
9XLOVD/RNJNrxkykViyzguDuPBYrRjfVocjefzKiXkGtTbJ3Lp7ezNjLXDsjlZoK
BiOhbQkswwKBgEldfANUuRL6VU7bAuAUW3DawZUpfDpQCRLqp2bDzJq+5kPgnl3w
TadBZqasMuO51pUDSPqF/6nJfT+fv/AtnJFrJxPK5rzU0EQdT1UXqzNl996c98dI
hbbBEJ39fXinEhu/0giICiguZzsuwpBfiDr+LVYbp5qNGFslNZhmdudnAoGAOyvp
TcyxzMhy3pFj5+mWYPlnFOYumvR4AJa3AAZVQMmvsTIs42kDsnG+vE+sWNnH5cC5
vPsKcpYE3m5VzBpTFLfAJ/x35ZRuhmS8vuNNatVRqzmTpPbUTo8YSE6jsEUVUuy5
6O+G/vO24yGX5e/EB+kX7jdsJxF/BJuZ3QuwD9ECgYB0bhOrVjpbfhxDfsvHbkb5
p7Onpp9JrxXevcdAhOgw/OmNxSgqE5TZjCXknPT4tPsOR1F/THMbtWMkADayua2i
2txxrlEB+I7tLEb3E8T6cF7aJn03Des8p9fcAM6yS6Sbsa0NHpjNf1DRzeeXf204
e451rpYJcee/1EhNRpvn6Q==
-----END PRIVATE KEY-----";

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth2JsonWebToken(
                    jwt,
                    privateKey,
                    clientId)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedArgsCount, response.Args.Count);
            Assert.NotNull(response.Args["assertion"]);
            Assert.Equal(clientId, response.Args[OAuth2Parameter.ClientId.EnumToString()]);
            Assert.Equal(GrantType.JsonWebToken.EnumToString(), response.Args[OAuth2Parameter.GrantType.EnumToString()]);
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

            var response = await new HttpRequestBuilder(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .ForOAuth2RefreshToken(
                    clientId,
                    clientSecret, 
                    refreshToken)
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
        public async void AddBearerHeader()
        {
            var bearer = Guid.NewGuid().ToString();
            var expected = $"Bearer {bearer}";

            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_getPath)
                .Bearer(bearer)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expected, response.Headers["Authorization"]);
        }

        [Fact]
        public async void CanGetSetCookies()
        {
            var expectedCookieName = Guid.NewGuid().ToString();
            var expectedCookieValue = Guid.NewGuid().ToString();

            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_cookiePath)
                .Parameter(
                    expectedCookieName, 
                    expectedCookieValue)
                .ExecuteAsync()
                .ConfigureAwait(false);

            var actualCookie = response
                .Cookies
                .SingleOrDefault(c => c.Name == expectedCookieName);

            Assert.NotNull(actualCookie);
            Assert.Equal(expectedCookieValue, actualCookie.Value);
        }

        [Fact]
        public async void HasUseragentString()
        {
            var expectedAgent = Guid.NewGuid().ToString();
            var expectedVersion = Guid.NewGuid().ToString();
            var expectedUserAgent = $"{expectedAgent}/{expectedVersion}";

            var response = await new HttpRequestBuilder(_endpoint)
                .GetFrom(_getPath)
                .UserAgent(expectedAgent, expectedVersion)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            Assert.Equal(expectedUserAgent, response.Headers["User-Agent"]);
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
