using System;
using System.Globalization;
using System.Net;
using Foundations.Enums;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Canonicalizers;
using Foundations.HttpClient.Extensions;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class CanonicalizationTests
    {
        [Fact]
        public void CanonicalizeOAuth1Request()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanonicalizeMicrosoftRequest()
        {
            var expected = "GET\n\napplication/json\nSun, 11 Oct 2009 21:49:13 GMT\n/MyAccount/Tables()";
            var accountName = "MyAccount";
            var baseAddress = "https://myaccount.table.core.windows.net/";
            var path = "Tables()";
            var timestamp = new DateTime(2009, 10, 11, 21, 49, 13, DateTimeKind.Utc)
                .ToString("R", CultureInfo.InvariantCulture);

            var request = new HttpRequestBuilder(baseAddress)
                .GetFrom(path)
                .Header(
                    HttpRequestHeader.ContentType, 
                    MediaType.Json.EnumToString())
                .Header("x-ms-date", timestamp)
                .Parameter("timeout", 20.ToString());

            var canonicalizer = new MicrosoftCanonicalizer(accountName);

            var actual = canonicalizer.CanonicalizeHttpRequest(request);

            Assert.Equal(expected, actual);
        }
    }
}
