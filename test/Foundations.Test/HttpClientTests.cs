using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Foundations.HttpClient;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Foundations.Test
{
    public class HttpClientTests
    {
        private const string _endpoint = "https://httpbin.org";
        private const string _getPath = "get";
        private const string _postPath = "post";

        #region Get Requests

        [Fact]
        public async void MakeBasicHttpGetRequest()
        {
            var response = await new HttpRequest(_endpoint)
                .GetFrom(_getPath)
                .ResultAsync()
                .ConfigureAwait(false);

            var actual = JObject.Parse(response);

            Assert.NotNull(actual["url"]?.ToString());
        }

        [Fact]
        public async void MakeBasicHttpGetRequestForceBody()
        {
            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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
                    await new HttpRequest(_endpoint)
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
                    await new HttpRequest(_endpoint)
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
                    await new HttpRequest(_endpoint)
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
                    await new HttpRequest(_endpoint)
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
                    await new HttpRequest(_endpoint)
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
            var response = await new HttpRequest(_endpoint)
                .PostTo(_postPath)
                .ResultAsync()
                .ConfigureAwait(false);

            var actual = JObject.Parse(response);

            Assert.NotNull(actual["url"]?.ToString());
        }

        [Fact]
        public async void MakeBasicHttpPostRequestForceQuerystring()
        {
            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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

            var response = await new HttpRequest(_endpoint)
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
        public async void MakeRequestWithUrlSegments()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void AcceptsAllTypes()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void AcceptsAllEncodingTypes()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void HasAllHeaderTypes()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void HasResponseMediaType()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void DoesNotGetAnHttpOkCodeThrowsException()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void AddAuthentication()
        {
            var expectedArgsCount = 7;
            var expected = new SampleBody
            {
                SomeKey = Guid.NewGuid().ToString()
            };
            var consumerKey = Guid.NewGuid().ToString();
            var consumerSecret = Guid.NewGuid().ToString();
            var callbackUrl = "http://localhost:8080";
            var authenticator = new OAuth1RequestToken(
                consumerKey,
                consumerSecret,
                callbackUrl);

            var response = await new HttpRequest(_endpoint)
                .PostTo(_postPath)
                .JsonContent(expected)
                .Authenticator(authenticator)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>()
                .ConfigureAwait(false);

            var actual = response.Json;

            Assert.Equal(expected.SomeKey, actual.SomeKey);
            Assert.Equal(expectedArgsCount, response.Args.Count);
        }

        [Fact]
        public async void HasUseragentString()
        {
            throw new NotImplementedException();
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
