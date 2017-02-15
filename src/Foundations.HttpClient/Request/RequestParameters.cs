using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Foundations.Enums;
using Foundations.HttpClient.Authenticators;

namespace Foundations.HttpClient.Request
{
    public class RequestParameters
    {
        public Uri Address { get; set; }

        public HttpMethod Method { get; set; }

        public RequestPayload Payload { get; } = new RequestPayload();

        public HeaderCollection Headers { get; } = new HeaderCollection();

        public CookieCollection Cookies { get; } = new CookieCollection();

        private HttpContent _content;

        public HttpContent Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (Method == HttpMethod.Get && value != null)
                {
                    throw new NotSupportedException(
                        StringResource.GetWithBodyNotSupported);
                }
                _content = value;
            }
        }

        public List<HttpStatusCode> ExpectedResponseCodes { get; } =
            new List<HttpStatusCode>();

        public MediaType? OverriddenMediaType { get; set; }

        public IAuthenticator Authenticator { get; set; }

        public bool AllowHttpRedirect { get; set; } = true;

        public void AddPath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var uriBuilder = new UriBuilder(Address);
            uriBuilder.Path += path.TrimStart('/');
            Address = uriBuilder.Uri;
        }

        public void AddPathParameter(string key, string value)
        {
            Address = new Uri(Address.ToString().Replace("{" + key + "}", value));
        }


    }
}
