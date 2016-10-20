using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient
{
    public class HttpRequestBuilder
    {
        private readonly Uri _baseAddress;
        private readonly HttpRequestMessage _message = 
            new HttpRequestMessage();
        private readonly HttpClientHandler _messageHandler =
            HttpConfiguration.MessageHandlerFactory();
        private readonly RequestPayload _payload = 
            new RequestPayload();
        private readonly List<KeyValuePair<string, string>> _pathParameters =
            new List<KeyValuePair<string, string>>();

        private string _path;
        private IAuthenticator _authenticator;
        private MediaType _responseType = MediaType.Undefined;
        private HttpStatusCode? _expectedResponseCode;

        public HttpMethod Method => _message.Method;

        public Uri Url => new Uri(_baseAddress + _path);

        public IEnumerable<KeyValuePair<string, string>> QueryParameters => 
            _payload.QueryParameters;

        public HttpRequestBuilder(Uri baseAddress) 
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            _baseAddress = baseAddress;

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
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _path = path.TrimStart('/');

            _message.Method = method;
            _payload.SetParameterHandling(parameterType);

            return this;
        }

        public HttpRequestBuilder Segments(
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            foreach (var parameter in parameters)
            {
                _pathParameters.Add(parameter);
            }

            return this;
        }

        public HttpRequestBuilder AcceptsDecompressionEncoding(
            DecompressionMethods decompressionMethod)
        {
            if (decompressionMethod == DecompressionMethods.None)
            {
                _message.Headers.AcceptEncoding.Clear();
            }

            _message.Headers.AcceptEncoding.Add(
                new StringWithQualityHeaderValue(
                    decompressionMethod.ToString().ToLower()));

            _messageHandler.AutomaticDecompression =
                _messageHandler.AutomaticDecompression |
                decompressionMethod;

            return this;
        }

        public HttpRequestBuilder Accepts(MediaType contentType)
        {
            _message.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(
                    contentType.EnumToString()));

            return this;
        }

        public HttpRequestBuilder Header(
            HttpRequestHeader header, 
            string value)
        {
            return Headers(new Dictionary<HttpRequestHeader, string>
            {
                {header, value}
            });
        }

        public HttpRequestBuilder Headers(
            IDictionary<HttpRequestHeader, string> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            foreach (var header in headers)
            {
                _message.Headers.Add(
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
                new Dictionary<string, string> { {key, value} });
        }

        public HttpRequestBuilder Parameters(
            IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            foreach (var parameter in parameters)
            {
                _payload.AddParameter(
                    parameter.Key,
                    parameter.Value);
            }

            return this;
        }

        public HttpRequestBuilder Content(
            object content,
            MediaType mediaType,
            Encoding encoding)
        {
            if (content == null)
            {
                return this;
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            _payload.AddContent(new Body()
            {
                Content = content,
                MediaType = mediaType,
                Encoding = encoding
            });

            return this;
        }

        public HttpRequestBuilder ResponseMediaType(
            MediaType mediaType)
        {
            _responseType = mediaType;

            return this;
        }

        public HttpRequestBuilder ThrowIfNotExpectedResponseCode(
            HttpStatusCode expectedResponseCode)
        {
            _expectedResponseCode = expectedResponseCode;

            return this;
        }

        public HttpRequestBuilder Authenticator(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;

            return this;
        }

        public HttpRequestBuilder UserAgent(
            string agent,
            string version)
        {
            _message.Headers.UserAgent.Add(
                new ProductInfoHeaderValue(
                    agent,
                    version));

            return this;
        }

        public HttpRequestBuilder PreventAutoRedirects()
        {
            _messageHandler.AllowAutoRedirect = false;

            return this;
        }

        public async Task<HttpResponse> ExecuteAsync()
        {
            using (var client = new System.Net.Http.HttpClient(_messageHandler))
            {
                _message.RequestUri = _baseAddress.AddPathParameters(
                    _path,
                    _pathParameters);
                
                _authenticator?.Authenticate(this);

                _payload.Attach(_message);

                if (_message.Method == HttpMethod.Get &&
                    _message.Content != null)
                {
                    throw new NotSupportedException(
                        StringResource.GetWithBodyNotSupported);
                }

                var response = await client
                    .SendAsync(_message)
                    .ConfigureAwait(false);

                if (_expectedResponseCode != null && 
                    _expectedResponseCode.Value != response.StatusCode)
                {
                    throw new HttpRequestException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            StringResource.BadHttpRequestException,
                            response.StatusCode,
                            _expectedResponseCode.Value,
                            response.ReasonPhrase));
                }

                return new HttpResponse(
                    response,
                    _messageHandler.CookieContainer.GetCookies(_baseAddress),
                    _responseType);
            }
        }
    }
}

