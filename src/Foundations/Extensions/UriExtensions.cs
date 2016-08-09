using System;
using System.Collections.Generic;

namespace Foundations.Extensions
{
    public static class UriExtensions
    {
        /// <summary>
        /// Gets a fragment of the uri containing the scheme and the host
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string NonPath(this Uri instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            return $"{instance.Scheme}://{instance.Authority}/";
        }

        public static Uri AddPathParameters(
            this Uri instance,
            string path,
            IEnumerable<KeyValuePair<string, string>> pathParameters)
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            var uriBuilder = new UriBuilder(instance);
            foreach (var segment in pathParameters)
            {
                path = path.Replace(
                    "{" + segment.Key + "}",
                    segment.Value);
            }
            uriBuilder.Path += path;
            return uriBuilder.Uri;
        }
    }
}
