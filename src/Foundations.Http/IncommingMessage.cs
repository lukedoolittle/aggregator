using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Foundations.Http
{
    public class IncommingMessage
    {
        private readonly HttpListenerRequest _request;

        public Uri Uri => _request.Url;

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

                using (var body = _request.InputStream)
                {
                    using (var reader = new StreamReader(body, _request.ContentEncoding))
                    {
                        _body = reader.ReadToEnd();
                    }
                }

                return _body;
            }
        }

        public ILookup<string, string> Query => _request.QueryString.ToLookup();

        public ILookup<string, string> Headers => _request.Headers.ToLookup();

        public IList<Cookie> Cookies => _request.Cookies.ToList();

        public IncommingMessage(HttpListenerRequest request)
        {
            _request = request;
        }
    }
}
