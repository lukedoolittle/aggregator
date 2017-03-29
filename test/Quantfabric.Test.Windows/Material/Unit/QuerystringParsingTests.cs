using System;
using Material.Framework.Collections;
using Xunit;

namespace Foundations.Test.Core
{
    [Trait("Category", "Continuous")]
    public class QuerystringParsingTests
    {
        [Fact]
        public void ParseBasicQuerystringCapturesAllInput()
        {
            var uri = new Uri("https://www.google.com/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8");
            var expectedCount = 4;

            var actual = HttpUtility.ParseQueryString(uri.Query);

            Assert.Equal(expectedCount, actual.Count);
            Assert.NotNull(actual["sourceid"]);
        }
    }
}
