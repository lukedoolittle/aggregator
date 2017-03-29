using System;
using System.Net;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace Material.HttpClient.Extensions
{
    public static class HeaderExtensions
    {
        public static HttpRequestBuilder Bearer(
            this HttpRequestBuilder instance, 
            string token)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Header(
                HttpRequestHeader.Authorization,
                StringExtensions.Concatenate(
                    OAuth2Parameter.BearerHeader.EnumToString(), 
                    token, 
                    " "));
        }

        public static HttpRequestBuilder AcceptsEncodingGZip(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.AcceptsDecompressionEncoding(
                DecompressionMethods.GZip);
        }

        public static HttpRequestBuilder AcceptsEncodingDeflate(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.AcceptsDecompressionEncoding(
                DecompressionMethods.Deflate);
        }

        public static HttpRequestBuilder AcceptsEncodingNone(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.AcceptsDecompressionEncoding(
                DecompressionMethods.None);
        }

        public static HttpRequestBuilder AcceptsJson(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.Json);
        }

        public static HttpRequestBuilder AcceptsXml(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.Xml);
        }

        public static HttpRequestBuilder AcceptsHtml(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.Html);
        }

        public static HttpRequestBuilder AcceptsText(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.Text);
        }

        public static HttpRequestBuilder AcceptsForm(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.Form);
        }

        public static HttpRequestBuilder AcceptsJavascript(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.Javascript);
        }

        public static HttpRequestBuilder AcceptsXJson(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.TextXJson);
        }

        public static HttpRequestBuilder AcceptsJsonText(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.TextJson);
        }

        public static HttpRequestBuilder AcceptsXmlText(
            this HttpRequestBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Accepts(MediaType.TextXml);
        }
    }
}
