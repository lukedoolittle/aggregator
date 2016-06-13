using System;
using Aggregator.Configuration;
using Xunit;

namespace Aggregator.Test.Unit
{
    
    public class ApplicationSettingsFixture
    {
        [Fact]
        public void CanWriteAnApplicationSettingAndThenReadThatSettingBack()
        {
            var expected = Guid.Empty;

            UserSettings.UserId = expected;
            var actual = UserSettings.UserId;

            Assert.Equal(expected, actual);

            expected = Guid.NewGuid();

            UserSettings.UserId = expected;
            actual = UserSettings.UserId;

            Assert.Equal(expected, actual);
        }
    }
}
