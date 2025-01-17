﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class RawHttpClientTests
    {
        private const string _endpoint = "https://httpbin.org/";
        private const string _getPath = "get";
        private const string _postPath = "post";
        private const string _gzipPath = "gzip";
        private const string _deflatePath = "deflate";
        private const string _statusPath = "status/{code}";
        private const string _cookiePath = "cookies/set";
        private const string _audioPath = "Material/TestData/brian.wav";

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
            var stream = File.OpenRead(_audioPath);
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
            var rawBytes = System.IO.File.ReadAllBytes(_audioPath);
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

        #region Multiple Requests

        [Fact]
        public async void MakeMultipleSynchronousRequestsToSameEndpoint()
        {
            var numberOfRequests = 5;
            var baseUrl = _endpoint;
            var path = _getPath;
            var method = HttpMethod.Get;
            var parameterType = HttpParameterType.Querystring;

            var results = new List<TypedHttpBinResponse<SampleBody>>();
            for (var i = 0; i < numberOfRequests; i++)
            {
                var result = await GenerateBasicRequest(
                    baseUrl,
                    path,
                    method,
                    parameterType).ConfigureAwait(false);
                results.Add(result);
            }

            foreach (var result in results)
            {
                Assert.True(result.Args.Count == 0);
            }
        }

        [Fact]
        public async void MakeMultipleAsynchronousRequestsToSameEndpoint()
        {
            var numberOfRequests = 5;
            var baseUrl = _endpoint;
            var path = _getPath;
            var method = HttpMethod.Get;
            var parameterType = HttpParameterType.Querystring;

            var tasks = new List<Task<TypedHttpBinResponse<SampleBody>>>();
            for (var i = 0; i < numberOfRequests; i++)
            {
                var result = GenerateBasicRequest(
                    baseUrl,
                    path,
                    method,
                    parameterType);
                tasks.Add(result);
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            foreach (var result in results)
            {
                Assert.True(result.Args.Count == 0);
            }
        }

        [Fact]
        public async void MakeMultipleConcurrentRequestsToDifferentEndpoint()
        {
            var task1 = new HttpRequestBuilder("http://www.google.com")
                .GetFrom("")
                .ExecuteAsync();

            var task2 = new HttpRequestBuilder("http://www.yahoo.com")
                .GetFrom("")
                .ExecuteAsync();

            var task3 = GenerateBasicRequest(
                _endpoint,
                _getPath,
                HttpMethod.Get,
                HttpParameterType.Querystring);

            var task4 = GenerateBasicRequest(
                _endpoint,
                _postPath,
                HttpMethod.Post,
                HttpParameterType.Body);

            await Task.WhenAll(task1, task2, task3, task4).ConfigureAwait(false);

            Assert.True(task3.Result.Args.Count == 0);
            Assert.True(task4.Result.Args.Count == 0);
        }

        private static Task<TypedHttpBinResponse<SampleBody>> GenerateBasicRequest(
            string baseUrl,
            string path,
            HttpMethod method,
            HttpParameterType parameterType)
        {
            return new HttpRequestBuilder(baseUrl)
                .Request(method, path, parameterType)
                .ResultAsync<TypedHttpBinResponse<SampleBody>>();
        }

        #endregion Multiple Requests

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
