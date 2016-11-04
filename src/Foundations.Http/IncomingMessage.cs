using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Foundations.Http
{
    public class IncomingMessage
    {
        private readonly HttpListenerRequest _request;

        public Uri Uri => _request.Url;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string Url => _request.RawUrl;

        private string _body;

        public string BodyAsString
        {
            get
            {
                if (_body != null || !_request.HasEntityBody)
                {
                    return _body;
                }

                using (var reader = new StreamReader(
                        _request.InputStream, 
                        _request.ContentEncoding))
                {
                    _body = reader.ReadToEnd();
                }

                return _body;
            }
        }

        public ILookup<string, string> Query => _request.QueryString.ToLookup();

        public ILookup<string, string> Headers => _request.Headers.ToLookup();

        public IList<Cookie> Cookies => _request.Cookies.ToList();

        public HttpMethod Method => new HttpMethod(_request.HttpMethod);

        public IncomingMessage(HttpListenerRequest request)
        {
            _request = request;
        }
    }
}
