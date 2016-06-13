using System;
using Aggregator.Framework.Extensions;
using Xunit;

namespace Aggregator.Test.Unit
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void TwitterIntoDateTimeOffset()
        {
            var time = "Tue Nov 03 01:58:26 -0000 2015";
            var format = "ddd MMM dd HH:mm:ss zzz yyyy";

            var actual = time.ToDateTimeOffset(format, null);

            AssertDateTimeOffset(actual, false);
        }

        [Fact]
        public void FacebookIntoDateTimeOffset()
        {
            var time = "2015-11-02T18:58:26-07:00";
            var format = "yyyy-MM-ddTHH:mm:sszzz";

            var actual = time.ToDateTimeOffset(format, null);

            AssertDateTimeOffset(actual, true);
        }

        [Fact]
        public void RunkeeperIntoDateTimeOffset()
        {
            var time = "Mon, 2 Nov 2015 18:58:26";
            var format = "ddd, d MMM yyyy HH:mm:ss";

            var actual = time.ToDateTimeOffset(format, "-7");

            AssertDateTimeOffset(actual, true);
        }

        [Fact]
        public void FitbitIntoDateTimeOffset()
        {
            var time = "2015-11-02 18:58:26";
            var format = "yyyy-MM-dd HH:mm:ss";
            var offset = "-25200000";

            var actual = time.ToDateTimeOffset(format, offset);

            AssertDateTimeOffset(actual, true);
        }

        [Fact]
        public void CurrentFitbitIntoDateTimeOffset()
        {
            var time = "2015-11-02 18:58:26 -0700";
            var format = "yyyy-MM-dd HH:mm:ss zzz";

            var actual = time.ToDateTimeOffset(format, null);

            AssertDateTimeOffset(actual, true);
        }

        [Fact(Skip = "Rescuetime doesn't correctly determine offset")]
        public void RescuetimeIntoDateTimeOffset()
        {
            var time = "11/02/2015 7:58:26 PM";
            var format = "MM/dd/yyyy h:mm:ss tt";

            var actual = time.ToDateTimeOffset(format, null);

            AssertDateTimeOffset(actual, false, true);
        }


        [Fact]
        public void GoogleIntoDateTimeOffset()
        {
            var time = "1446515906000";
            var offset = "0000";

            var actual = time.ToDateTimeOffset(null, offset);

            AssertDateTimeOffset(actual, false);
        }

        [Fact]
        public void FoursquareIntoDateTimeOffset()
        {
            var time = "1446515906";
            var offset = "-420";

            var actual = time.ToDateTimeOffset(null, offset);

            AssertDateTimeOffset(actual, true);
        }

        [Fact]
        public void SpotifyIntoDateTimeOffset()
        {
            var time = "2015-11-03T01:58:26Z";
            var format = "yyyy-MM-ddTHH:mm:ssZ";

            var actual = time.ToDateTimeOffset(format, null);

            AssertDateTimeOffset(actual, false);
        }

        public static void AssertDateTimeOffset(DateTimeOffset actual, bool offset, bool local = false)
        {
            Assert.Equal(0, actual.Offset.Days);
            var localoffset = DateTimeOffset.Now.Offset.Hours;
            if (offset)
            {
                Assert.Equal(-7, actual.Offset.Hours);
            }
            else if (local)
            {
                Assert.Equal(localoffset, actual.Offset.Hours);
            }
            else
            {
                Assert.Equal(0, actual.Offset.Hours);
            }

            Assert.Equal(0, actual.Offset.Minutes);
            Assert.Equal(0, actual.Offset.Seconds);

            var datetime = actual.UtcDateTime;

            Assert.Equal(1, datetime.Hour);
            Assert.Equal(58, datetime.Minute);
            Assert.Equal(26, datetime.Second);
            Assert.Equal(3, datetime.Day);
            Assert.Equal(11, datetime.Month);
            Assert.Equal(2015, datetime.Year);
        }
    }
}
