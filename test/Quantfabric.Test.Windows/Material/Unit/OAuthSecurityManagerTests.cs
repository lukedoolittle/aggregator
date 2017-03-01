using System;
using System.Security;
using System.Threading;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.OAuth.Security;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class OAuthSecurityManagerTests
    {
        [Fact]
        public void GettingParameterValueForUserTwiceReturnsSameParameterValue()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            var expected = target.CreateOrGetSecureParameter(
                userId, 
                parameterName);

            Assert.NotNull(expected);

            var actual = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GettingParameterValueForUserTwiceAfterExpirationReturnsDifferentParameterValue()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromSeconds(1));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            var expected = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            Assert.NotNull(expected);
            Thread.Sleep(2000);

            var actual = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void CheckingParameterValueWhenNoParameterValueExistsReturnsFalse()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();
            var parameterValue = Guid.NewGuid().ToString();

            var actual = target.IsSecureParameterValid(
                userId, 
                parameterName, 
                parameterValue);

            Assert.False(actual);
        }

        [Fact]
        public void CheckingNoParameterValueWhenParameterValueExistsReturnsFalse()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            var actual = target.IsSecureParameterValid(
                userId,
                parameterName,
                null);

            Assert.False(actual);
        }

        [Fact]
        public void SettingSecureParameterAndThenGettingParameterReturnsSameValue()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();
            var expected = Guid.NewGuid().ToString();

            target.SetSecureParameter(
                userId, 
                parameterName, 
                expected);

            var actual = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SettingSecureParameterTwiceThrowsException()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();
            var expected = Guid.NewGuid().ToString();

            target.SetSecureParameter(
                userId,
                parameterName,
                expected);

            Assert.Throws<SecurityException>(() => 
                target.SetSecureParameter(
                    userId,
                    parameterName,
                    expected));
        }

        [Fact]
        public void CheckingNoParameterValueWhenNoParameterValueExistsReturnsFalse()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            var actual = target.IsSecureParameterValid(
                userId,
                parameterName,
                null);

            Assert.False(actual);
        }

        [Fact]
        public void CheckingMatchingButExpiredParameterValueReturnsFalse()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromSeconds(1));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            var expected = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            Thread.Sleep(2000);

            var actual = target.IsSecureParameterValid(
                userId,
                parameterName,
                expected);

            Assert.False(actual);
        }

        [Fact]
        public void CheckingNonmatchingParameterValueReturnsFalse()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            var expected = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            var actual = target.IsSecureParameterValid(
                userId,
                parameterName,
                Guid.NewGuid().ToString());

            Assert.False(actual);
        }

        [Fact]
        public void CheckingMatchingParameterValueReturnsTrue()
        {
            var target = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();
            var parameterName = OAuth2Parameter.State.EnumToString();

            var expected = target.CreateOrGetSecureParameter(
                userId,
                parameterName);

            var actual = target.IsSecureParameterValid(
                userId,
                parameterName,
                expected);

            Assert.True(actual);
        }
    }
}
