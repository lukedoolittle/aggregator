using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class HttpRequestBuilder : IDisposable
    {
        private readonly Uri _baseAddress;
        private readonly HttpRequestMessage _message = 
            new HttpRequestMessage();
        private readonly HttpClientHandler _messageHandler =
            HttpConfiguration.MessageHandlerFactory();
        private readonly RequestPayload _payload = 
            new RequestPayload();
        private readonly HttpValueCollection _pathParameters =
            new HttpValueCollection();

        private string _path;
        private IAuthenticator _authenticator;
        private MediaType _responseType = MediaType.Undefined;
        private HttpStatusCode? _expectedResponseCode;
        private bool _hasDefaultAccepts;

        public HttpMethod Method => _message.Method;

        public Uri Url => new Uri(_baseAddress + _path);

        public HttpValueCollection QueryParameters => 
            _payload.QueryParameters;

        public HttpRequestBuilder(Uri baseAddress) 
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            _baseAddress = baseAddress;

            HttpConfiguration.DefaultBuilderSetup(this);
            _hasDefaultAccepts = true;
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

            _path = path.TrimStart('/');

            _message.Method = method;
            _payload.SetParameterHandling(parameterType);

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
                if (header.Key == HttpRequestHeader.Accept && 
                    _hasDefaultAccepts)
                {
                    _message.Headers.Accept.Clear();
                    _hasDefaultAccepts = false;
                }

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
                _payload.AddParameter(
                    parameter.Key,
                    parameter.Value);
            }

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

            _payload.AddContent(new Body()
            {
                Content = bodyContent,
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
            IAuthenticator requestAuthenticator)
        {
            _authenticator = requestAuthenticator;

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
                    var content = await response
                        .Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                    throw new HttpRequestException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            StringResource.BadHttpRequestException,
                            response.StatusCode,
                            _expectedResponseCode.Value,
                            content));
                }

                return new HttpResponse(
                    response,
                    _messageHandler.CookieContainer.GetCookies(_baseAddress),
                    _responseType);
            }
        }

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HttpRequestBuilder()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free managed resources
                (_message as IDisposable).Dispose();
                (_messageHandler as IDisposable).Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
    }
}

