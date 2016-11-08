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

        public HttpContent Content { get; set; }

        public List<HttpStatusCode> ExpectedResponseCodes { get; } =
            new List<HttpStatusCode>();

        public MediaType? OverriddenMediaType { get; set; }

        public IAuthenticator Authenticator { get; set; }

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

        public HeaderCollection Headers { get; } = 
            new HeaderCollection();
    }
}
