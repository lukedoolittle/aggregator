using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace Foundations.Http
{
    public class ServerResponse
    {
        private readonly HttpListenerResponse _response;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly StringBuilder _responseBody = new StringBuilder();

        private int _statusCode;

        private byte[] _responseBuffer => 
            Encoding.UTF8.GetBytes(_responseBody.ToString());

        public ServerResponse(HttpListenerResponse response)
        {
            _response = response;
        }

        public void WriteHead(HttpStatusCode statusCode)
        {
            WriteHead((int) statusCode);
        }

        public void WriteHead(int statusCode)
        {
            _statusCode = statusCode;
        }

        public void WriteHead(
            HttpRequestHeader header, 
            MediaType headerContent)
        {
            WriteHead(header, headerContent.EnumToString());
        }

        public void WriteHead(HttpRequestHeader header, string headerContent)
        {
            if (headerContent == null)
            {
                throw new ArgumentNullException(nameof(headerContent));
            }

            switch (header)
            {
                case HttpRequestHeader.ContentType: 
                    WriteHead("Content-Type", headerContent);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void WriteHead(
            string headerType, 
            string headerContent)
        {
            WriteHead(new HeaderPair(headerType, headerContent));
        }

        public void WriteHead(params HeaderPair[] headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            foreach (var header in headers)
            {
                _headers.Add(header.Key, header.Value);
            }
        }

        public void Write(string data)
        {
            _responseBody.Append(data);
        }

        public void WriteHtmlString(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            WriteHead(HttpStatusCode.OK);
            WriteHead(HttpRequestHeader.ContentType, "text/html");
            Write(html);
            End();
        }

        public void Redirect(Uri newUrl)
        {
            if (newUrl == null)
            {
                throw new ArgumentNullException(nameof(newUrl));
            }

            WriteHead(HttpStatusCode.Redirect);
            WriteHead("Location", newUrl.ToString());
            End();
        }

        public void End(bool failSilently)
        {
            try
            {
                foreach (var header in _headers)
                {
                    _response.AddHeader(header.Key, header.Value);
                }
                _response.StatusCode = _statusCode;
                _response.OutputStream.Write(_responseBuffer, 0, _responseBuffer.Length);
                _response.Close();
            }
            catch (HttpListenerException)
            {
                if (!failSilently)
                {
                    throw;
                }
            }
        }

        public void End()
        {
            End(true);
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
