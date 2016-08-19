using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Foundations.Http
{
    public class IncommingMessage
    {
        public Uri Uri { get; }

        public string Url { get; }

        public ILookup<string, string> Query { get; }

        public ILookup<string, string> Headers { get; }

        public IList<Cookie> Cookies { get; }

    }
}
