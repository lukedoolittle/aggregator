using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace Foundations.HttpClient.Request
{
    public class HeaderCollection : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> _items = 
            new List<KeyValuePair<string, object>>();

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

        public void AddAcceptsEncoding(object value)
        {
            _items.Add(new KeyValuePair<string, object>(
                HttpRequestHeader.AcceptEncoding.ToString(), 
                value));
        }

        public void AddUserAgent(object value)
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
