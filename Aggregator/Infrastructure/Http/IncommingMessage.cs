using System;
using System.Collections.Specialized;
using System.Net;

namespace Aggregator.Task.Http
{
    public class IncommingMessage
    {
        private readonly HttpListenerRequest _request;

        public Uri Uri => _request.Url;

        public string Url => _request.RawUrl;

        public NameValueCollection Query => _request.QueryString;

        public IncommingMessage(HttpListenerRequest request)
        {
            _request = request;
        }
    }
}
