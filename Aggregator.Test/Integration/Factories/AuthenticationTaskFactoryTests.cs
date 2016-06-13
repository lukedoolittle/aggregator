using System;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Framework.Contracts;
using Aggregator.Task;
using Aggregator.Task.Authentication;
using Aggregator.Test.Helpers;
using Xunit;
#if __IOS__
using UIKit;
#endif
#if __ANDROID__
using Android.App;
#endif

namespace Aggregator.Test.Integration.Factories
{
    using Aggregator.Infrastructure.Services;

    public class AuthenticationTaskFactoryTests : IClassFixture<BootstrapFixture>
    {
        private readonly IServiceLocator _resolver;

        public AuthenticationTaskFactoryTests(BootstrapFixture fixture)
        {
            _resolver = fixture.Resolver;
        }
#if !__MOBILE__
        [Fact]
        public void GenerateOAuth1Task()
        {
            var factory =
                _resolver.GetInstance<WindowsAuthenticationTaskFactory>();
            var task = factory.GenerateTask<Twitter>(Guid.NewGuid(), -1);
            
            Assert.True(task is OAuth1AuthenticationTask<Twitter>);
            Assert.NotNull(task);
        }
#endif
#if __IOS__
        [Fact]
        public void GenerateOAuth2Task()
        {
            var factory =
                _resolver.GetInstance<iOSAuthenticationTaskFactory>();
            var task = factory.GenerateTask<Facebook>(Guid.NewGuid(), -1, GetContext() as UIViewController);

            Assert.True(task is OAuth2AuthenticationTask<Facebook>);
            Assert.NotNull(task);
        }
#endif

        [Fact]
        public void GenerateOAuth2RefreshTask()
        {
            var factory =
                _resolver.GetInstance<IAuthenticationRefreshTaskFactory>();
            var task = factory.GenerateRefreshTask(typeof(Google), Guid.NewGuid());

            Assert.True(task is RefreshTokenTask<Google>);
            Assert.NotNull(task);
        }

        [Fact]
        public void TryToGenerateOAuth1RefreshTaskReturnsNull()
        {
            var factory =
                _resolver.GetInstance<IAuthenticationRefreshTaskFactory>();

            var refreshTask = factory.GenerateRefreshTask(typeof(Twitter), Guid.NewGuid());

            Assert.Null(refreshTask);
        }

#if __ANDROID____
        [Fact]
        public void GenerateNonAuthenticatedServiceTaskThrowsException()
        {
            var factory =
                _resolver.GetInstance<AndroidAuthenticationTaskFactory>();
            Assert.Throws<NotSupportedException>(() => 
                factory.GenerateTask<SMS>(Guid.NewGuid(), -1, GetContext() as Activity));
        }

        [Fact]
        public void GenerateBluetoothTask()
        {
            var factory =
                _resolver.GetInstance<AndroidAuthenticationTaskFactory>();
            var task = factory.GenerateTask<Mioalpha>(Guid.NewGuid(), -1, GetContext() as Activity);

            Assert.True(task is BluetoothAuthenticationTask<Mioalpha>);
            Assert.NotNull(task);
        }
#endif

        private object GetContext()
        {
#if __ANDROID__
            return new Activity();
#elif __IOS__
            return new UIKit.UIViewController();
#else
            return null;
#endif
        }
    }
}
