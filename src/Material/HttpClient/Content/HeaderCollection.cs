using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Material.HttpClient.Content
{
    public class HeaderCollection : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> _items = 
            new List<KeyValuePair<string, object>>();

        public string this[string key]
        {
            get
            {
                return string.Join(
                    ",", 
                    _items
                        .Where(i => i.Key == key)
                        .Select(x => x.Value.ToString())
                        .ToArray());
            }
        }

        public string this[HttpRequestHeader key] => this[key.ToString()];

        public void AttachHeaders(HttpRequestHeaders headers)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            //This is very fragile; do you have to do this???
            foreach (var header in _items)
            {
                if (header.Key == HttpRequestHeader.Accept.ToString())
                {
                    headers.Accept.Add((MediaTypeWithQualityHeaderValue)header.Value);
                }
                else if (header.Key == HttpRequestHeader.AcceptEncoding.ToString())
                {
                    headers.AcceptEncoding.Add((StringWithQualityHeaderValue)header.Value);
                }
                else if (header.Key == HttpRequestHeader.UserAgent.ToString())
                {
                    headers.UserAgent.Add((ProductInfoHeaderValue)header.Value);
                }
                else
                {
                    headers.Add(
                        header.Key,
                        header.Value.ToString());
                }
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string key, string value)
        {
            _items.Add(new KeyValuePair<string, object>(key, value));
        }

        public void AddAccepts(MediaTypeWithQualityHeaderValue value)
        {
            _items.Add(new KeyValuePair<string, object>(
                HttpRequestHeader.Accept.ToString(), 
                value));
        }

        public void AddAcceptsEncoding(StringWithQualityHeaderValue value)
        {
            _items.Add(new KeyValuePair<string, object>(
                HttpRequestHeader.AcceptEncoding.ToString(), 
                value));
        }

        public void AddUserAgent(ProductInfoHeaderValue value)
        {
            _items.Add(new KeyValuePair<string, object>(
                HttpRequestHeader.UserAgent.ToString(),
                value));
        }

        public void Clear(string key)
        {
            foreach (var item in _items)
            {
                if (item.Key == key)
                {
                    _items.Remove(item);
                }
            }
        }

        public void Clear(HttpRequestHeader header)
        {
            Clear(header.ToString());
        }
    }
}
