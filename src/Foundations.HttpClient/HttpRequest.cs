using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.Http;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Foundations.HttpClient.ParameterHandlers;

namespace Foundations.HttpClient
{
    public class HttpRequest
    {
        private readonly HttpRequestMessage _message = 
            new HttpRequestMessage();
        private readonly List<KeyValuePair<string, string>> _parameters = 
            new List<KeyValuePair<string, string>>();

        private string _path;
        private IAuthenticator _authenticator;
        private IParameterHandler _parameterHandler;

        public HttpRequest PostTo(string path)
        {
            _path = path.TrimStart('/');

            _message.Method = HttpMethod.Post;
            _parameterHandler = new PostParameterHandler();

            return this;
        }

        public HttpRequest GetTo(string path)
        {
            _path = path.TrimStart('/');

            _message.Method = HttpMethod.Get;
            _parameterHandler = new GetParameterHandler();

            return this;
        }

        public HttpRequest Accepts(string contentType)
        {
            _message.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(contentType));

            return this;
        }

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

        public HttpRequest Parameter(
            string key, 
            string value)
        {
            _parameters.Add(
                new KeyValuePair<string, string>(
                    key, 
                    value));

            return this;
        }

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

        public HttpRequest Authenticator(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;

            return this;
        }

        public async Task<HttpResponse> ExecuteAsync(Uri baseAddress)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var uriBuilder = new UriBuilder(baseAddress);
                uriBuilder.Path += _path;

                client.BaseAddress = uriBuilder.Uri;
                
                _authenticator?.Authenticate(this);

                if (_message.Content == null)
                {
                    _parameterHandler.AddParameters(
                        _message,
                        MediaTypeEnum.Form, 
                        _parameters);
                }
                else
                {
                    throw new NotImplementedException();
                }

                var response = await client
                    .SendAsync(_message)
                    .ConfigureAwait(false);

                return new HttpResponse(
                    response.Content,
                    response.Headers,
                    response.StatusCode,
                    response.ReasonPhrase);
            }
        }
    }
}

