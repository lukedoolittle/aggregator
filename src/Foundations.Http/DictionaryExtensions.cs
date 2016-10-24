using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace Foundations.Http
{
    public static class DictionaryExtensions
    {
        public static ILookup<string, string> ToLookup(
            this NameValueCollection instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.AllKeys.ToLookup(a => a, a => instance[a]);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<Cookie> ToList(
            this CookieCollection instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var cookies = new List<Cookie>();

            for (var i = 0; i < instance.Count; i++)
            {
                cookies.Add(instance[i]);
            }

            return cookies;
        }
    }
}
