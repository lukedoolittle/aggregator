using System;
using System.Globalization;
using Foundations.HttpClient;
using Foundations.HttpClient.Canonicalizers;
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
            var expected = "GET\n\n\n\n\nx-ms-date:Sun, 11 Oct 2009 21:49:13 GMT\nx-ms-version:2009-09-19\n/myaccount/ mycontainer\ncomp:metadata\nrestype:container\ntimeout:20";
            var accountName = Guid.NewGuid().ToString();
            var baseAddress = "https://myaccount.table.core.windows.net/";
            var timestamp = new DateTime(2009, 10, 11, 21, 49, 13, DateTimeKind.Utc)
                .ToString("R", CultureInfo.InvariantCulture);

            var request = new HttpRequestBuilder(baseAddress)
                .Parameter("x-ms-date", timestamp)
                .Parameter("timeout", 20.ToString());

            var canonicalizer = new MicrosoftCanonicalizer(accountName);

            var actual = canonicalizer.CanonicalizeHttpRequest(request);

            Assert.Equal(expected, actual);
        }
    }
}
