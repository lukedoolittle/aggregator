using System.Collections.Generic;
using Foundations.Extensions;
using Xunit;

namespace Foundations.Test.Core
{
    [Trait("Category", "Continuous")]
    public class TypeExtensionTests
    {
        [Fact]
        public void GetPrettyPrintedGenericNameWithTypes()
        {
            var target = typeof(List<string>);
            var expected = "List<String>";

            var actual = target.GetTypedGenericName();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrettyPrintedGenericNameWithoutTypes()
        {
            var target = typeof(List<string>);
            var expected = "List";

            var actual = target.GetNonGenericName();

            Assert.Equal(expected, actual);
        }
    }
}
