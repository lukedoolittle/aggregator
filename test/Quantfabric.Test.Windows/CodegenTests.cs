using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    public class CodegenTests
    {
        [Fact]
        public void StringWithNoSpecialCharacters()
        {
            var target = "someNewVar";
            var expected = "SomeNewVar";

            var actual = CreateCSharpPropertyName(target);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StringWithHyphens()
        {
            var target = "some-new-var";
            var expected = "SomeNewVar";

            var actual = CreateCSharpPropertyName(target);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StringWithUnderscores()
        {
            var target = "some_new_var";
            var expected = "SomeNewVar";

            var actual = CreateCSharpPropertyName(target);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StringWithTrailingOrLeadingUnderscores()
        {
            var target = "-some_new_var-";
            var expected = "SomeNewVar";

            var actual = CreateCSharpPropertyName(target);

            Assert.Equal(expected, actual);
        }

        public string CreateCSharpPropertyName(string jsonName)
        {
            if (string.IsNullOrEmpty(jsonName))
            {
                return string.Empty;
            }

            var result = jsonName.Split(new[] { "_", "-" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

            return char.ToUpper(result[0]) + result.Substring(1);
        }
    }
}
