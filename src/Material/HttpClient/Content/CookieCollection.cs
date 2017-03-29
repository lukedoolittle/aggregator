using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Material.HttpClient.Content
{
    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly List<Cookie> _cookies = 
            new List<Cookie>();

        public void AttachCookies(
            CookieContainer container, 
            Uri address)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            foreach (var cookie in _cookies)
            {
                container.Add(
                    address,
                    cookie);
            }
        }

        public void AddRange(IEnumerable<Cookie> cookies)
        {
            _cookies.AddRange(cookies);
        }

        public IEnumerator<Cookie> GetEnumerator()
        {
            return _cookies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
