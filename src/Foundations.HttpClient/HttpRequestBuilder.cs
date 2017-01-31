using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Collections;
using Foundations.Enums;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient
{
    public class HttpRequestBuilder
    {
        private readonly Func<RequestParameters, Tuple<System.Net.Http.HttpClient, HttpClientHandler>> _clientFactory;
        private readonly RequestParameters _request = 
            new RequestParameters();

        public HttpMethod Method => _request.Method;
        public Uri Url => _request.Address;
        public HttpValueCollection QueryParameters =>  _request.Payload.QueryParameters;

        public HttpRequestBuilder(Uri baseAddress) 
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            _request.Address = baseAddress;

            _clientFactory = HttpConfiguration.HttpClientFactory;
            HttpConfiguration.DefaultBuilderSetup(this);
        }

        public HttpRequestBuilder(string baseAddress) : 
            this(new Uri(baseAddress))
        { }

        public HttpRequestBuilder Request(
            HttpMethod method,
            string path,
            HttpParameterType parameterType)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (path == null) throw new ArgumentNullException(nameof(path));

            _request.AddPath(path);

            _request.Method = method;
            _request.Payload.SetParameterHandling(parameterType);

            return this;
        }

        public HttpRequestBuilder Segments(
            HttpValueCollection parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            foreach (var parameter in parameters)
            {
                _request.AddPathParameter(
                    parameter.Key, 
                    parameter.Value);
            }

            return this;
        }

        public HttpRequestBuilder AcceptsDecompressionEncoding(
            DecompressionMethods decompressionMethod)
        {
            _request.Headers.AddAcceptsEncoding(
                new StringWithQualityHeaderValue(
                    decompressionMethod.ToString().ToLower()));

            return this;
        }

        public HttpRequestBuilder Accepts(MediaType contentType)
        {
            _request.Headers.AddAccepts(
                new MediaTypeWithQualityHeaderValue(
                    contentType.EnumToString()));

            return this;
        }

        public HttpRequestBuilder Header(
            string name, 
            string value)
        {
            _request.Headers.Add(name, value);

            return this;
        }

        public HttpRequestBuilder Header(
            HttpRequestHeader httpHeader, 
            string value)
        {
            return Headers(new Dictionary<HttpRequestHeader, string>
            {
                {httpHeader, value}
            });
        }

        public HttpRequestBuilder Headers(
            IDictionary<HttpRequestHeader, string> httpHeaders)
        {
            if (httpHeaders == null)
            {
                throw new ArgumentNullException(nameof(httpHeaders));
            }

            foreach (var header in httpHeaders)
            {
                _request.Headers.Add(
                    header.Key.ToString(), 
                    header.Value);
            }

            return this;
        }

        public HttpRequestBuilder Parameter(
            string key,
            string value)
        {
            return Parameters(
                new HttpValueCollection { { key, value } });
        }

        public HttpRequestBuilder Parameters(
            HttpValueCollection requestParameters)
        {
            if (requestParameters == null)
            {
                throw new ArgumentNullException(nameof(requestParameters));
            }

            foreach (var parameter in requestParameters)
            {
                _request.Payload.AddParameter(
                    parameter.Key,
                    parameter.Value);
            }

            return this;
        }

        public HttpRequestBuilder RawContent(
            byte[] value,
            MediaType mediaType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            _request.Payload.AddContent(
                new RawContent(
                    value,
                    mediaType));

            return this;
        }

        public HttpRequestBuilder StreamingContent(
            Stream stream, 
            MediaType mediaType)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _request.Payload.AddContent(
                new StreamingContent(
                    stream, 
                    mediaType));

            return this;
        }

        public HttpRequestBuilder Content(
            object bodyContent,
            MediaType mediaType,
            Encoding encoding)
        {
            if (bodyContent == null)
            {
                return this;
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            _request.Payload.AddContent(
                new SerializableContent(
                    bodyContent, 
                    encoding, 
                    mediaType));

            return this;
        }

        public HttpRequestBuilder OverrideResponseMediaType(
            MediaType? mediaType)
        {
            _request.OverriddenMediaType = mediaType;

            return this;
        }

        public HttpRequestBuilder ThrowIfNotExpectedResponseCode(
            HttpStatusCode expectedResponseCode)
        {
            _request.ExpectedResponseCodes.Add(expectedResponseCode);

            return this;
        }

        public HttpRequestBuilder Authenticator(
            IAuthenticator requestAuthenticator)
        {
            _request.Authenticator = requestAuthenticator;

            return this;
        }

        public HttpRequestBuilder UserAgent(
            string agent,
            string version)
        {
            _request.Headers.AddUserAgent(
                new ProductInfoHeaderValue(
                    agent,
                    version));

            return this;
        }

        public HttpRequestBuilder DisableAutoRedirect()
        {
            _request.AllowHttpRedirect = false;

            return this;
        }

        public Uri GenerateRequestUri()
        {
            _request.Authenticator?.Authenticate(this);
            _request.Payload.Attach(_request);

            return _request.Address;
        }

        public async Task<HttpResponse> ExecuteAsync()
        {
            _request.Authenticator?.Authenticate(this);
            _request.Payload.Attach(_request);

            if (_request.Method == HttpMethod.Get &&
                _request.Content != null)
            {
                throw new NotSupportedException(
                    StringResource.GetWithBodyNotSupported);
            }

            var client = _clientFactory(_request);

            var message = new HttpRequestMessage
            {
                Method = _request.Method,
                RequestUri = _request.Address,
                Content = _request.Content
            };
            //This is very fragile; do you have to do this???
            foreach (var header in _request.Headers)
            {
                if (header.Key == HttpRequestHeader.Accept.ToString())
                {
                    message.Headers.Accept.Add((MediaTypeWithQualityHeaderValue)header.Value);
                }
                else if (header.Key == HttpRequestHeader.AcceptEncoding.ToString())
                {
                    message.Headers.AcceptEncoding.Add((StringWithQualityHeaderValue)header.Value);
                }
                else if (header.Key == HttpRequestHeader.UserAgent.ToString())
                {
                    message.Headers.UserAgent.Add((ProductInfoHeaderValue)header.Value);
                }
                else
                {
                    message.Headers.Add(
                        header.Key,
                        header.Value.ToString());
                }
            }
            
            var response = await client.Item1
                .SendAsync(message)
                .ConfigureAwait(false);

            if (_request.ExpectedResponseCodes.Count > 0 &&
                !_request.ExpectedResponseCodes.Contains(response.StatusCode))
            {
                var content = await response
                    .Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                throw new HttpRequestException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResource.BadHttpRequestException,
                        response.StatusCode,
                        _request.ExpectedResponseCodes.First(),
                        content));
            }

            if (_request.OverriddenMediaType != null)
            {
                return new HttpResponse(
                    response,
                    client.Item2.CookieContainer.GetCookies(_request.Address),
                    _request.OverriddenMediaType.Value);
            }
            else
            {
                return new HttpResponse(
                    response,
                    client.Item2.CookieContainer.GetCookies(_request.Address));
            }
        }
    }
}

