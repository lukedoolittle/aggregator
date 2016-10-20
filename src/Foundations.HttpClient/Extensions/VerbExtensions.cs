using System;
using System.Collections.Generic;
using System.Net.Http;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Extensions
{
    public static class VerbExtensions
    {
        public static HttpRequestBuilder Segment(
            this HttpRequestBuilder instance,
            string key, 
            string value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Segments(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(key, value)
                });
        }

        public static HttpRequestBuilder Request(
            this HttpRequestBuilder instance,
            string method,
            string path,
            HttpParameterType parameterType)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Request(
                new HttpMethod(method),
                path,
                parameterType);
        }

        public static HttpRequestBuilder PostTo(
            this HttpRequestBuilder instance, 
            string path)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.PostTo(
                path,
                HttpParameterType.Body);
        }

        public static HttpRequestBuilder PostTo(
            this HttpRequestBuilder instance,
            string path,
            HttpParameterType parameterType)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Request(
                HttpMethod.Post,
                path,
                parameterType);
        }

        public static HttpRequestBuilder GetFrom(
            this HttpRequestBuilder instance, 
            string path)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.GetFrom(
                path,
                HttpParameterType.Querystring);
        }

        public static HttpRequestBuilder GetFrom(
            this HttpRequestBuilder instance,
            string path,
            HttpParameterType parameterType)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Request(
                HttpMethod.Get,
                path,
                parameterType);
        }
    }
}
