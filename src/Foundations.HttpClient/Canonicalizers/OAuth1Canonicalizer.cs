using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.Collections;
using Foundations.Extensions;

namespace Foundations.HttpClient.Canonicalizers
{
    public class OAuth1Canonicalizer : IHttpRequestCanonicalizer
    {
        public string CanonicalizeHttpRequest(
            HttpRequestBuilder request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var parameters = EncodeAndSortParameters(
                new HttpValueCollection(request.QueryParameters));

            var elements = new List<string>
            {
                request.Method.ToString(),
                request.Url.ToString().UrlEncodeString(),
                parameters.Concatenate("=", "&").UrlEncodeString()
            };

            return elements.Concatenate("&");
        }

        private static HttpValueCollection EncodeAndSortParameters(
            HttpValueCollection instance)
        {
            var encodedParameters = instance
                .EncodeParameters()
                .ToList();

            encodedParameters.Sort((x, y) =>
                string.CompareOrdinal(x.Key, y.Key) != 0
                    ? string.CompareOrdinal(x.Key, y.Key)
                    : string.CompareOrdinal(x.Value, y.Value));

            return encodedParameters;
        }
    }
}
