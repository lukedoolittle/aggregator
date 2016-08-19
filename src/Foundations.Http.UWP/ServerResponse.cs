using System;
using System.Net;

namespace Foundations.Http
{
    public class ServerResponse
    {
        public void WriteHead(HttpStatusCode statusCode)
        {
            throw new NotImplementedException();
        }

        public void WriteHead(int statusCode)
        {

        }

        public void WriteHead(
            HttpRequestHeader header,
            MediaTypeEnum headerContent)
        {
            throw new NotImplementedException();
        }

        public void WriteHead(HttpRequestHeader header, string headerContent)
        {
            throw new NotImplementedException();
        }

        public void WriteHead(
            string headerType,
            string headerContent)
        {
            throw new NotImplementedException();
        }

        public void WriteHead(params HeaderPair[] headers)
        {
            throw new NotImplementedException();
        }

        public void Write(string data)
        {
            throw new NotImplementedException();
        }

        public void WriteHtmlString(string html)
        {
            throw new NotImplementedException();
        }

        public void WriteHtml(string fileName)
        {
            throw new NotImplementedException();
        }

        public void End(bool failSilently = true)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderPair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public HeaderPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
