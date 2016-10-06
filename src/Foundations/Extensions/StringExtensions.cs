using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Foundations.Extensions
{
    public static class StringExtensions
    {
        public static string ToBase64String(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(instance);
            return Convert.ToBase64String(plainTextBytes);
        }

        //Adapted from https://github.com/restsharp/RestSharp/blob/master/RestSharp/Authenticators/OAuth/OAuthTools.cs

        /// <summary>
        /// Sorts a collection of key-value pairs by name, and then value if equal,
        /// concatenating them into a single string. This string should be encoded
        /// prior to, or after normalization is run.
        /// </summary>
        /// <seealso cref="http://oauth.net/core/1.0#rfc.section.9.1.1"/>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> Normalize(
            this IEnumerable<KeyValuePair<string, string>> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var encodedParameters = instance.Select(p =>
                new KeyValuePair<string, string>(
                    p.Key.UrlEncodeStrict(),
                    p.Value.UrlEncodeStrict()))
                .ToList();

            encodedParameters.Sort((x, y) => string.CompareOrdinal(x.Key, y.Key) != 0
                ? string.CompareOrdinal(x.Key, y.Key)
                : string.CompareOrdinal(x.Value, y.Value));

            return encodedParameters;
        }


        private const string DIGIT = "1234567890";
        private const string LOWER = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string ALPHA_NUMERIC = UPPER + LOWER + DIGIT;
        private const string UNRESERVED = ALPHA_NUMERIC + "-._~";
        /// <summary>
        /// URL encodes a string based on section 5.1 of the OAuth spec.
        /// Namely, percent encoding with [RFC3986], avoiding unreserved characters,
        /// upper-casing hexadecimal characters, and UTF-8 encoding for text value pairs.
        /// </summary>
        /// <param name="value"></param>
        /// <seealso cref="http://oauth.net/core/1.0#encoding_parameters" />
        public static string UrlEncodeStrict(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            // From oauth spec above: -
            // Characters not in the unreserved character set ([RFC3986]
            // (Berners-Lee, T., "Uniform Resource Identifiers (URI):
            // Generic Syntax," .) section 2.3) MUST be encoded.
            // ...
            // unreserved = ALPHA, DIGIT, '-', '.', '_', '~'
            string result = "";

            foreach (char c in instance)
            {
                result += UNRESERVED.Contains(c.ToString())
                    ? c.ToString()
                    : PercentEncode(c.ToString());
            };

            return result;
        }

        private static string PercentEncode(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                sb.Append(string.Format(CultureInfo.InvariantCulture, "%{0:X2}", b));
            }

            return sb.ToString();
        }

        /// <summary>
        /// The set of characters that are unreserved in RFC 2396 but are NOT unreserved in RFC 3986.
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/846487/how-to-get-uri-escapedatastring-to-comply-with-rfc-3986" />
        private static readonly string[] uriRfc3986CharsToEscape = { "!", "*", "'", "(", ")" };

        private static readonly string[] uriRfc3968EscapedHex = { "%21", "%2A", "%27", "%28", "%29" };

        public static string UrlEncodeRelaxed(this string instance)
        {
            if (instance == null)
            {
                return string.Empty;
            }
            // Start with RFC 2396 escaping by calling the .NET method to do the work.
            // This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
            // If it does, the escaping we do that follows it will be a no-op since the
            // characters we search for to replace can't possibly exist in the string.
            StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(instance));

            // Upgrade the escaping to RFC 3986, if necessary.
            for (int i = 0; i < uriRfc3986CharsToEscape.Length; i++)
            {
                string t = uriRfc3986CharsToEscape[i];

                escaped.Replace(t, uriRfc3968EscapedHex[i]);
            }

            // Return the fully-RFC3986-escaped string.
            return escaped.ToString();
        }
    }
}



