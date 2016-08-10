using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Extensions;
using Foundations.Serialization;
using Newtonsoft.Json;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    public class JObjectExtensionTests
    {
        private readonly JToken _data = JToken.Parse(@"{""something"": {""property"": [ ""value"", ""value2"", ""value3"" ],""timestampWithOffset"": ""Tue Nov 03 01:58:26 -0000 2015"", ""timestampNoOffset"": ""1446515906000"", ""timestampOffset"": ""0000"" }, ""property"" :  ""value""}");

        [Fact]
        public void GetMatchingPropertiesWithMultipleMatchesReturnsAllMatches()
        {
            var data = _data as JContainer;

            var actual = data.AllMatchingProperties("property");

            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void PuttingJObjectInContainerReturnsProperContainer()
        {
            var data = _data;

            var actual = data.InContainer();

            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public void PuttingJArrayInContainerReturnsProperContainer()
        {
            var data = _data["something"]["property"];

            var actual = data.InContainer();

            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public void PuttingJValueInContainerReturnsProperContainer()
        {
            var data = _data["property"];

            var actual = data.InContainer();

            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public void PuttingJTokenInContainerThrowsException()
        {
            var data = (_data["something"] as JObject).First;

            Assert.Throws<JsonSerializationException>(() => data.InContainer());
        }

        [Fact]
        public void ExtractTimestampWithNoNavigationSetsMinimumDefaultValue()
        {
            var data = _data;

            string navigation = null;

            var actual = data.ExtractTimestamp(navigation, null, null, null);

            Assert.Equal(DateTimeOffset.MinValue, actual);
        }

        [Fact]
        public void ExtractTimestampWithNoGivenOffset()
        {
            var data = _data;

            var navigation = "something.timestampWithOffset";
            var format = "ddd MMM dd HH:mm:ss zzz yyyy";

            var actual = data.ExtractTimestamp(navigation, format, null, null);

            DateTimeExtensionsTests.AssertDateTimeOffset(actual, false);
        }

        [Fact]
        public void ExtractTimestampWithGivenOffset()
        {
            var data = _data;
            var navigation ="something.timestampNoOffset";
            var offset = "0000";

            var actual = data.ExtractTimestamp(navigation, null, null, offset);

            DateTimeExtensionsTests.AssertDateTimeOffset(actual, false);
        }

        [Fact]
        public void ExtractTimestampWithGivenOffsetNavigation()
        {
            var data = _data;
            var navigation =  "something.timestampNoOffset";
            var offsetNavigation = "something.timestampOffset";

            var actual = data.ExtractTimestamp(navigation, null, offsetNavigation, null);

            DateTimeExtensionsTests.AssertDateTimeOffset(actual, false);
        }
    }
}
