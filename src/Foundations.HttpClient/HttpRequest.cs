using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.Exceptions;
using Foundations.HttpClient.ParameterHandlers;
using Foundations.HttpClient.Serialization;

namespace Foundations.HttpClient
{
    public class HttpRequest
    {
        private readonly Uri _baseAddress;
        private readonly HttpRequestMessage _message = 
            new HttpRequestMessage();
        private readonly HttpClientHandler _messageHandler;
        private readonly CookieContainer _cookies =
            new CookieContainer();
        private readonly List<KeyValuePair<string, string>> _queryParameters = 
            new List<KeyValuePair<string, string>>();
        private readonly List<KeyValuePair<string, string>> _pathParameters =
            new List<KeyValuePair<string, string>>();

        private readonly Dictionary<MediaType, ISerializer> _serializers =
            new Dictionary<MediaType, ISerializer>
            {
                { MediaType.Json, new JsonSerializer() },
                { MediaType.TextJson, new JsonSerializer() },
                { MediaType.TextXJson, new JsonSerializer() },
                { MediaType.Xml, new XmlSerializer() },
                { MediaType.TextXml, new XmlSerializer() },
                { MediaType.Html, new HtmlSerializer() },
                { MediaType.Text, new HtmlSerializer() },
                { MediaType.Form, new HtmlSerializer() },
                { MediaType.Javascript, new JsonSerializer() }
            };

        private string _path;
        private IAuthenticator _authenticator;
        private IParameterHandler _parameterHandler;
        private MediaType _responseType = MediaType.Undefined;
        private HttpStatusCode? _expectedResponseCode;

        public HttpMethod Method => _message.Method;

        public Uri Url => new Uri(_baseAddress + _path);

        public IEnumerable<KeyValuePair<string, string>> QueryParameters => _queryParameters;

        public HttpRequest(Uri baseAddress) : 
            this(
                baseAddress, 
                new HttpClientHandler())
        { }

        public HttpRequest(
            Uri baseAddress,
            HttpClientHandler messageHandler)
        {
            _baseAddress = baseAddress;
            _messageHandler = messageHandler;
            _messageHandler.CookieContainer = _cookies;
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

        public HttpRequest Request(
            HttpMethod method, 
            string path,
            HttpParameterType parameterType)
        {
            if (method == HttpMethod.Get)
            {
                return GetFrom(path, parameterType);
            }
            else if (method == HttpMethod.Post)
            {
                return PostTo(path, parameterType);
            }
            else
            {
                throw new NotSupportedException();
            }
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
            _path = path.TrimStart('/');

            _message.Method = HttpMethod.Post;
            _parameterHandler = GetParameterHandler(parameterType);

            return this;
        }

        public HttpRequest GetFrom(
            string path,
            HttpParameterType parameterType)
        {
            _path = path.TrimStart('/');

            _message.Method = HttpMethod.Get;
            _parameterHandler = GetParameterHandler(parameterType);

            return this;
        }

        private IParameterHandler GetParameterHandler(
            HttpParameterType parameterType)
        {
            switch (parameterType)
            {
                case HttpParameterType.Body:
                    return new PostParameterHandler();
                case HttpParameterType.Querystring:
                    return new GetParameterHandler();
                case HttpParameterType.Header:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        //TODO: change above to accept an override to parameter handling
        //then get rid of this
        public HttpRequest WithQueryParameters()
        {
            _parameterHandler = new GetParameterHandler();

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

        public HttpRequest DisallowAutoRedirects()
        {
            _messageHandler.AllowAutoRedirect = false;

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
            _queryParameters.Add(
                new KeyValuePair<string, string>(
                    key, 
                    value));

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
                _queryParameters.Add(parameter);
            }

            return this;
        }

        #endregion Parameters

        #region Content

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

            if (_message.Method != HttpMethod.Post)
            {
                throw new HttpRequestContentException();
            }

            var serializedContent = _serializers[mediaType]
                .Serialize(content);

            _message.Content = new StringContent(
                serializedContent, 
                encoding, 
                mediaType.EnumToString());

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

        public async Task<HttpResponse> ExecuteAsync()
        {
            using (var client = new System.Net.Http.HttpClient(_messageHandler))
            {
                AddDefaults();

                _message.RequestUri = _baseAddress.AddPathParameters(
                    _path,
                    _pathParameters);
                
                _authenticator?.Authenticate(this);

                _parameterHandler.AddParameters(
                    _message,
                    MediaType.Form, 
                    _queryParameters);

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
                    response.Content,
                    response.Headers,
                    response.StatusCode,
                    response.ReasonPhrase,
                    _cookies.GetCookies(_baseAddress).Cast<Cookie>(),
                    GetSerializer(response));
            }
        }

        private ISerializer GetSerializer(HttpResponseMessage response)
        {
            var resultContentType = _responseType != MediaType.Undefined ?
                                        _responseType :
                                        response
                                            .Content
                                            .Headers
                                            .ContentType
                                            .MediaType
                                            .StringToEnum<MediaType>();

            return _serializers.ContainsKey(resultContentType) ? 
                        _serializers[resultContentType] : 
                        null;
        }

        private void AddDefaults()
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

            if (ContainsNoneEncoding())
            {
                _message.Headers.AcceptEncoding.Clear();
            }
            else if (_message.Headers.AcceptEncoding.Count == 0)
            {
                AcceptsEncodingGzip()
                    .AcceptsEncodingDeflate();
            }
        }

        private bool ContainsNoneEncoding()
        {
            var noneHeaders = _message.Headers.AcceptEncoding.Count(i =>
                i.Value == DecompressionMethods.None.ToString().ToLower());

            return noneHeaders >= 1;
        }
    }
}

