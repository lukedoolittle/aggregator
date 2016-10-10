using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
                throw new ArgumentNullException(nameof(instance));
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}://{1}/", instance.Scheme, instance.Authority);
        }

        public static string NoQuerystring(this Uri instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}://{1}{2}",
                instance.Scheme, 
                instance.Authority,
                instance.AbsolutePath);
        }

        /// <summary>
        /// Adds path with parameters to Uri
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="pathParameters"></param>
        /// <returns></returns>
        public static Uri AddPathParameters(
            this Uri instance,
            string path,
            IEnumerable<KeyValuePair<string, string>> pathParameters)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (pathParameters == null)
            {
                throw new ArgumentNullException(nameof(pathParameters));
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

        /// <summary>
        /// Add parameters as a url encoded querystring
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public static Uri AddEncodedQuerystring(
            this Uri instance,
            IEnumerable<KeyValuePair<string, string>> queryParameters)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var querstring = queryParameters.ToDictionary(
                    d => d.Key.UrlEncodeStrict(),
                    d => d.Value.UrlEncodeStrict())
                .Concatenate("=", "&");

            var uriBuilder = new UriBuilder(instance)
            {
                Query = querstring
            };
            return uriBuilder.Uri;
        }
    }
}
