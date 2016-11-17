using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Foundations.Collections;

namespace Foundations.Extensions
{
    public static class StringExtensions
    {
        public static string Concatenate(
            string first, 
            string second, 
            string separator)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (separator == null) throw new ArgumentNullException(nameof(separator));

            return new StringBuilder()
                .Append(first)
                .Append(separator)
                .Append(second)
                .ToString();
        }

        public static byte[] BytesFromBase64String(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return Convert.FromBase64String(
                instance.ToProperBase64String());
        }

        public static string FromBase64String(this string instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var bytes = instance.BytesFromBase64String();

            return Encoding.UTF8.GetString(
                bytes, 
                0, 
                bytes.Length - 1);
        }

        public static string ToProperBase64String(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            instance = instance.Replace('-', '+').Replace('_', '/').Replace("\r", "");

            while (instance.Length % 4 != 0)
            {
                instance = instance + "=";
            }

            return instance;
        }

        public static string ToBase64String(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(instance);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static HttpValueCollection EncodeAndSortParameters(
            this HttpValueCollection instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var encodedParameters = instance.EncodeParameters().ToList();

            encodedParameters.Sort((x, y) => 
                string.CompareOrdinal(x.Key, y.Key) != 0
                    ? string.CompareOrdinal(x.Key, y.Key)
                    : string.CompareOrdinal(x.Value, y.Value));

            return encodedParameters;
        }

        //Adapted from https://github.com/xamarin/Xamarin.Auth/blob/master/src/Xamarin.Auth/OAuth1.cs
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string UrlEncodeString(this string instance)
        {
            if (instance == null)
            {
                return string.Empty;
            }

            var bytes = Encoding.UTF8.GetBytes(instance);
            var stringBuilder = new StringBuilder();

            foreach (var v in bytes)
            {
                if ((0x41 <= v && v <= 0x5A) || 
                    (0x61 <= v && v <= 0x7A) || 
                    (0x30 <= v && v <= 0x39) ||
                    v == 0x2D || 
                    v == 0x2E || 
                    v == 0x5F || 
                    v == 0x7E)
                {
                    stringBuilder.Append((char)v);
                }
                else
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture, 
                        "%{0:X2}", 
                        v);
                }
            }

            return stringBuilder.ToString();
        }
    }
}



