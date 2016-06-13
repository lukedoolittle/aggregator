using System;
using System.Linq;
using BatmansBelt.Extensions;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Exceptions;
using Aggregator.Framework.Extensions;
using Aggregator.Test.Fixtures;
using Xunit;

namespace Aggregator.Test.Unit
{
    public class JObjectExtensionTests : IClassFixture<SampleJsonFixture>
    {
        private readonly SampleJsonFixture _fixture;

        public JObjectExtensionTests(SampleJsonFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetMatchingPropertiesWithMultipleMatchesReturnsAllMatches()
        {
            var data = _fixture.SampleJson as JContainer;

            var actual = data.AllMatchingProperties("property");

            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void PuttingJObjectInContainerReturnsProperContainer()
        {
            var data = _fixture.SampleJson;

            var actual = data.InContainer();

            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public void PuttingJArrayInContainerReturnsProperContainer()
        {
            var data = _fixture.SampleJson["something"]["property"];

            var actual = data.InContainer();

            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public void PuttingJValueInContainerReturnsProperContainer()
        {
            var data = _fixture.SampleJson["property"];

            var actual = data.InContainer();

            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public void PuttingJTokenInContainerThrowsException()
        {
            var data = (_fixture.SampleJson["something"] as JObject).First;

            Assert.Throws<JsonResponseFormatException>(() => data.InContainer());
        }

        [Fact]
        public void ExtractTimestampWithNoNavigationSetsMinimumDefaultValue()
        {
            var data = _fixture.SampleJson;

            string navigation = null;

            var actual = data.ExtractTimestamp(navigation, null, null, null);

            Assert.Equal(DateTimeOffset.MinValue, actual);
        }

        [Fact]
        public void ExtractTimestampWithNoGivenOffset()
        {
            var data = _fixture.SampleJson;

            var navigation = "something.timestampWithOffset";
            var format = "ddd MMM dd HH:mm:ss zzz yyyy";

            var actual = data.ExtractTimestamp(navigation, format, null, null);

            DateTimeExtensionsTests.AssertDateTimeOffset(actual, false);
        }

        [Fact]
        public void ExtractTimestampWithGivenOffset()
        {
            var data = _fixture.SampleJson;
            var navigation ="something.timestampNoOffset";
            var offset = "0000";

            var actual = data.ExtractTimestamp(navigation, null, null, offset);

            DateTimeExtensionsTests.AssertDateTimeOffset(actual, false);
        }

        [Fact]
        public void ExtractTimestampWithGivenOffsetNavigation()
        {
            var data = _fixture.SampleJson;
            var navigation =  "something.timestampNoOffset";
            var offsetNavigation = "something.timestampOffset";

            var actual = data.ExtractTimestamp(navigation, null, offsetNavigation, null);

            DateTimeExtensionsTests.AssertDateTimeOffset(actual, false);
        }
    }
}
