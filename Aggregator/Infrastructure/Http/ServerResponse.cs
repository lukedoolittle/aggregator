using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using BatmansBelt.Extensions;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Extensions;

namespace Aggregator.Task.Http
{
    using System.Threading.Tasks;

    public class ServerResponse
    {
        private readonly HttpListenerResponse _response;
        private readonly Dictionary<string, string> _headers;
        private readonly StringBuilder _responseBody;

        private int _statusCode;

        private byte[] _responseBuffer => 
            Encoding.UTF8.GetBytes(_responseBody.ToString());

        public ServerResponse(HttpListenerResponse response)
        {
            _response = response;

            _headers = new Dictionary<string, string>();
            _responseBody = new StringBuilder();
        }

        public void WriteHead(HttpStatusCode statusCode)
        {
            WriteHead((int) statusCode);
        }

        public void WriteHead(int statusCode)
        {
            _statusCode = statusCode;
        }

        public void WriteHead(HttpRequestHeader header, MimeTypeEnum headerContent)
        {
            WriteHead(header, headerContent.EnumToString());
        }

        public void WriteHead(HttpRequestHeader header, string headerContent)
        {
            switch (header)
            {
                case HttpRequestHeader.ContentType: 
                    WriteHead("Content-Type", headerContent);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void WriteHead(string headerType, string headerContent)
        {
            WriteHead(new HeaderPair(headerType, headerContent));
        }

        public void WriteHead(params HeaderPair[] headers)
        {
            foreach (var header in headers)
            {
                _headers.Add(header.Key, header.Value);
            }
        }

        public void Write(string data)
        {
            _responseBody.Append(data);
        }

        public void WriteHtml(string fileName)
        {
            WriteHead(HttpStatusCode.OK);
            WriteHead(HttpRequestHeader.ContentType, "text/html");
#if !__ANDROID__
            Write(File.ReadAllText(fileName));
#else
            string content;
            using (StreamReader sr = new StreamReader(
                Android.App.Application.Context.Assets.Open(fileName)))
            {
                content = sr.ReadToEnd();
            }
            Write(content);
#endif
            End();
        }

        public void End(bool failSilently = true)
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
                //by default we want a silent failure because the response from the server
                //doesn't really and the response may be disposed of before we close it

                if (!failSilently)
                {
                    throw;
                }
            }
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
