using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.Http;
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
        private readonly List<KeyValuePair<string, string>> _queryParameters = 
            new List<KeyValuePair<string, string>>();
        private readonly List<KeyValuePair<string, string>> _pathParameters =
            new List<KeyValuePair<string, string>>();
        private readonly HttpClientHandler _messageHandler;

        private readonly Dictionary<MediaTypeEnum, ISerializer> _serializers =
            new Dictionary<MediaTypeEnum, ISerializer>
            {
                { MediaTypeEnum.Json, new JsonSerializer() },
                { MediaTypeEnum.TextJson, new JsonSerializer() },
                { MediaTypeEnum.TextXJson, new JsonSerializer() },
                { MediaTypeEnum.Xml, new XmlSerializer() },
                { MediaTypeEnum.TextXml, new XmlSerializer() },
                { MediaTypeEnum.Html, new HtmlSerializer() },
                { MediaTypeEnum.Text, new HtmlSerializer() },
                { MediaTypeEnum.Javascript, new JsonSerializer() }
            };


        private string _path;
        private IAuthenticator _authenticator;
        private IParameterHandler _parameterHandler;
        private MediaTypeEnum _responseType = MediaTypeEnum.Undefined;

        public HttpMethod Method => _message.Method;

        public Uri Url => new Uri(_baseAddress + _path);

        public IEnumerable<KeyValuePair<string, string>> QueryParameters => _queryParameters;

        public HttpRequest(
            Uri baseAddress,
            HttpClientHandler messageHandler = null)
        {
            _baseAddress = baseAddress;
            _messageHandler = messageHandler ?? new HttpClientHandler();
        }

        public HttpRequest(string baseAddress) : 
            this(new Uri(baseAddress))
        { }

        #region Method and Path

        public HttpRequest Request(HttpMethod method, string path)
        {
            if (method == HttpMethod.Get)
            {
                return GetFrom(path);
            }
            else if (method == HttpMethod.Post)
            {
                return PostTo(path);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public HttpRequest PostTo(string path)
        {
            _path = path.TrimStart('/');

            _message.Method = HttpMethod.Post;
            _parameterHandler = new PostParameterHandler();

            return this;
        }

        public HttpRequest GetFrom(string path)
        {
            _path = path.TrimStart('/');

            _message.Method = HttpMethod.Get;
            _parameterHandler = new GetParameterHandler();

            return this;
        }

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

        #endregion Method and Path

        #region Accepts

        public HttpRequest Accepts(MediaTypeEnum contentType)
        {
            _message.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(
                    contentType.EnumToString()));

            return this;
        }

        public HttpRequest AcceptsJson()
        {
            return Accepts(MediaTypeEnum.Json);
        }

        public HttpRequest AcceptsXml()
        {
            return Accepts(MediaTypeEnum.Xml);
        }

        public HttpRequest AcceptsHtml()
        {
            return Accepts(MediaTypeEnum.Html);
        }

        public HttpRequest AcceptsText()
        {
            return Accepts(MediaTypeEnum.Text);
        }

        public HttpRequest AcceptsForm()
        {
            return Accepts(MediaTypeEnum.Form);
        }

        public HttpRequest AcceptsJavascript()
        {
            return Accepts(MediaTypeEnum.Javascript);
        }

        public HttpRequest AcceptsXJson()
        {
            return Accepts(MediaTypeEnum.TextXJson);
        }

        public HttpRequest AcceptsJsonText()
        {
            return Accepts(MediaTypeEnum.TextJson);
        }

        public HttpRequest AcceptsXmlText()
        {
            return Accepts(MediaTypeEnum.TextXml);
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
            string content, 
            MediaTypeEnum mediaType)
        {
            return Content(
                content, 
                mediaType.EnumToString(), 
                Encoding.UTF8);
        }

        public HttpRequest Content(
            string content,
            string mediaType,
            Encoding encoding)
        {
            _message.Content = new StringContent(
                content, 
                encoding, 
                mediaType);

            return this;
        }

        #endregion Content

        public HttpRequest ResponseMediaType(MediaTypeEnum mediaType)
        {
            _responseType = mediaType;

            return this;
        }

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

                if (_message.Content == null)
                {
                    _parameterHandler.AddParameters(
                        _message,
                        MediaTypeEnum.Form, 
                        _queryParameters);
                }
                else
                {
                    if (_queryParameters.Count > 0)
                    {
                        throw new HttpRequestContentException();
                    }
                }

                var response = await client
                    .SendAsync(_message)
                    .ConfigureAwait(false);

                return new HttpResponse(
                    response.Content,
                    response.Headers,
                    response.StatusCode,
                    response.ReasonPhrase,
                    GetSerializer(response));
            }
        }

        private ISerializer GetSerializer(HttpResponseMessage response)
        {
            var resultContentType = _responseType != MediaTypeEnum.Undefined ?
                                        _responseType :
                                        response
                                            .Content
                                            .Headers
                                            .ContentType
                                            .MediaType
                                            .StringToEnum<MediaTypeEnum>();

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

