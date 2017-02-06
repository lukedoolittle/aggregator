using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Foundations.Collections;

namespace Foundations.HttpClient.Canonicalizers
{
    public class MicrosoftCanonicalizer : IHttpRequestCanonicalizer
    {
        private readonly string _accountName;

        public MicrosoftCanonicalizer(string accountName)
        {
            _accountName = accountName;
        }

        public string CanonicalizeHttpRequest(HttpRequestBuilder request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return string.Join(
                "\n", 
                new List<string>
                {
                    request.Method.Method,
                    request.RequestHeaders[HttpRequestHeader.ContentMd5],
                    request.RequestHeaders[HttpRequestHeader.ContentType],
                    request.RequestHeaders["x-ms-date"],
                    GetCanonicalizedResourceString(request.Url, _accountName)
                });
        }

        //Adapted from https://github.com/Azure/azure-storage-net/blob/afc5c6d99a805fb4165631ab5cfd139238b00e71/Lib/Common/Core/Util/AuthenticationUtility.cs
        private static string GetCanonicalizedResourceString(
            Uri uri, 
            string accountName)
        {
            var canonicalizedResource = new StringBuilder();
            canonicalizedResource.Append('/');
            canonicalizedResource.Append(accountName);
            canonicalizedResource.Append(GetAbsolutePathWithoutSecondarySuffix(uri, accountName));

            var queryParameters = HttpUtility.ParseQueryString(uri.Query);
            if (queryParameters.ContainsKey("comp"))
            {
                canonicalizedResource.Append("?comp=");
                canonicalizedResource.Append(queryParameters["comp"]);
            }

            return canonicalizedResource.ToString();
        }

        private static string GetAbsolutePathWithoutSecondarySuffix(
            Uri uri, 
            string accountName)
        {
            var secondaryLocationAccountSuffix = "-secondary";

            var absolutePath = uri.AbsolutePath;
            var secondaryAccountName = string.Concat(accountName, secondaryLocationAccountSuffix);

            var startIndex = absolutePath.IndexOf(secondaryAccountName, StringComparison.OrdinalIgnoreCase);
            if (startIndex == 1)
            {
                startIndex += accountName.Length;
                absolutePath = absolutePath.Remove(startIndex, secondaryLocationAccountSuffix.Length);
            }

            return absolutePath;
        }
    }
}
