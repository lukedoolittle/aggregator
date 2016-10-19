using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient
{
    public class HttpRequest
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

        public HttpRequest(Uri baseAddress) 
        {
            _baseAddress = baseAddress;

            _messageHandler.CookieContainer = new CookieContainer();

            AcceptsEncodingGzip().AcceptsEncodingDeflate();
        }

        public HttpRequest(string baseAddress) : 
            this(new Uri(baseAddress))
        { }

        #region Method and Path

        public HttpRequest Request(
            string method, 
            string path,
            HttpParameterType parameterType)
        {
            return Request(
                new HttpMethod(method), 
                path,
                parameterType);
        }

        public HttpRequest PostTo(string path)
        {
            return PostTo(
                path, 
                HttpParameterType.Body);
        }

        public HttpRequest PostTo(
            string path, 
            HttpParameterType parameterType)
        {
            return Request(
                HttpMethod.Post, 
                path, 
                parameterType);
        }

        public HttpRequest GetFrom(string path)
        {
            return GetFrom(
                path, 
                HttpParameterType.Querystring);
        }

        public HttpRequest GetFrom(
            string path,
            HttpParameterType parameterType)
        {
            return Request(
                HttpMethod.Get,
                path,
                parameterType);
        }

        private HttpRequest Request(
            HttpMethod method,
            string path,
            HttpParameterType parameterType)
        {
            _path = path.TrimStart('/');

            _message.Method = method;
            _payload.SetParameterHandling(parameterType);

            return this;
        }

        public HttpRequest Segments(
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            foreach (var parameter in parameters)
            {
                _pathParameters.Add(parameter);
            }

            return this;
        }

        #endregion Method and Path

        #region Accepts

        public HttpRequest Accepts(MediaType contentType)
        {
            _message.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(
                    contentType.EnumToString()));

            return this;
        }

        public HttpRequest AcceptsJson()
        {
            return Accepts(MediaType.Json);
        }

        public HttpRequest AcceptsXml()
        {
            return Accepts(MediaType.Xml);
        }

        public HttpRequest AcceptsHtml()
        {
            return Accepts(MediaType.Html);
        }

        public HttpRequest AcceptsText()
        {
            return Accepts(MediaType.Text);
        }

        public HttpRequest AcceptsForm()
        {
            return Accepts(MediaType.Form);
        }

        public HttpRequest AcceptsJavascript()
        {
            return Accepts(MediaType.Javascript);
        }

        public HttpRequest AcceptsXJson()
        {
            return Accepts(MediaType.TextXJson);
        }

        public HttpRequest AcceptsJsonText()
        {
            return Accepts(MediaType.TextJson);
        }

        public HttpRequest AcceptsXmlText()
        {
            return Accepts(MediaType.TextXml);
        }

        #endregion Accepts

        #region AcceptsEncoding

        public HttpRequest AcceptsEncodingGzip()
        {
            _message.Headers.AcceptEncoding.Add(
                new StringWithQualityHeaderValue(
                    DecompressionMethods.GZip.ToString().ToLower()));

            _messageHandler.AutomaticDecompression =
                _messageHandler.AutomaticDecompression |
                DecompressionMethods.GZip;

            return this;
        }

        public HttpRequest AcceptsEncodingDeflate()
        {
            _message.Headers.AcceptEncoding.Add(
                new StringWithQualityHeaderValue(
                    DecompressionMethods.Deflate.ToString().ToLower()));

            _messageHandler.AutomaticDecompression =
                _messageHandler.AutomaticDecompression |
                DecompressionMethods.Deflate;

            return this;
        }

        public HttpRequest AcceptsEncodingNone()
        {
            _message.Headers.AcceptEncoding.Clear();

            _message.Headers.AcceptEncoding.Add(
                new StringWithQualityHeaderValue(
                    DecompressionMethods.None.ToString().ToLower()));

            _messageHandler.AutomaticDecompression =
                DecompressionMethods.None;

            return this;
        }

        #endregion AcceptsEncoding

        #region Headers
        public HttpRequest Bearer(string token)
        {
            _message.Headers.Add(
                HttpRequestHeader.Authorization.ToString(),
                $"{OAuth2ParameterEnum.BearerHeader.EnumToString()} {token}");

            return this;
        }

        public HttpRequest Header(HttpRequestHeader header, string value)
        {
            return Headers(new Dictionary<HttpRequestHeader, string>
            {
                {header, value}
            });
        }

        public HttpRequest Headers(Dictionary<HttpRequestHeader, string> headers)
        {
            foreach (var header in headers)
            {
                _message.Headers.Add(
                    header.Key.ToString(), 
                    header.Value);
            }

            return this;
        }
        #endregion Headers

        #region Parameters

        public HttpRequest Parameter(
            string key, 
            string value)
        {
            _payload.AddParameter(
                    key, 
                    value);

            return this;
        }

        public HttpRequest Parameter(
            Enum key,
            string value)
        {
            return Parameter(
                key.EnumToString(), 
                value);
        }

        public HttpRequest Parameters(
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            foreach (var parameter in parameters)
            {
                _payload.AddParameter(
                    parameter.Key, 
                    parameter.Value);
            }

            return this;
        }

        public HttpRequest ParameterFromObject(object item)
        {
            var parameters = item
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Name,
                        x.GetValue(item, null).ToString()));

            _payload.AddParameters(parameters);

            return this;
        }

        #endregion Parameters

        #region Content

        public HttpRequest JsonContent(object content)
        {
            return Content(
                content, 
                MediaType.Json);
        }

        public HttpRequest Content(
            object content, 
            MediaType mediaType)
        {
            return Content(
                content, 
                mediaType, 
                Encoding.UTF8);
        }

        public HttpRequest Content(
            object content,
            MediaType mediaType,
            Encoding encoding)
        {
            if (content == null)
            {
                return this;
            }

            _payload.AddContent(new Body()
            {
                Content = content,
                MediaType = mediaType,
                Encoding = encoding
            });

            return this;
        }

        #endregion Content

        #region Response

        public HttpRequest ResponseMediaType(MediaType mediaType)
        {
            _responseType = mediaType;

            return this;
        }

        public HttpRequest ThrowIfNotExpectedResponseCode(
            HttpStatusCode expectedResponseCode)
        {
            _expectedResponseCode = expectedResponseCode;

            return this;
        }

        #endregion Response

        public HttpRequest Authenticator(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;

            return this;
        }

        public HttpRequest UserAgent(
            string agent,
            string version)
        {
            _message.Headers.UserAgent.Add(
                new ProductInfoHeaderValue(
                    agent,
                    version));

            return this;
        }

        public HttpRequest PreventAutoRedirects()
        {
            _messageHandler.AllowAutoRedirect = false;

            return this;
        }

        #region Request

        public async Task<string> ResultAsync()
        {
            var result = await ExecuteAsync().ConfigureAwait(false);

            return await result.ContentAsync().ConfigureAwait(false);
        }

        public async Task<T> ResultAsync<T>()
        {
            var result = await ExecuteAsync().ConfigureAwait(false);

            return await result.ContentAsync<T>().ConfigureAwait(false);
        }

        public async Task<HttpResponse> ExecuteAsync()
        {
            using (var client = new System.Net.Http.HttpClient(_messageHandler))
            {
                if (_message.Headers.Accept.Count == 0)
                {
                    AcceptsJson()
                        .AcceptsXml()
                        .AcceptsJsonText()
                        .AcceptsXmlText()
                        .AcceptsXJson()
                        .AcceptsJavascript();
                }

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
                    throw new HttpRequestException(string.Format(
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

        #endregion Requests

    }
}

