using System.Linq;
using Material.Application;
using Material.Contracts;
using Material.Infrastructure.ProtectedResources;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    [Trait("Category", "Automated")]
    public class SimplePasswordTests
    {
        private readonly AppCredentialRepository _appRepository =
            new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void CanGetValidCredentialsFromXamarinInsights()
        {
            var username = _appRepository.GetUsername<XamarinInsights>();
            var password = _appRepository.GetPassword<XamarinInsights>();

            var token = await new SimplePassword<XamarinInsights>(
                    username,
                    password)
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            Assert.True(token.Cookies.Count() == 2);
            Assert.True(token.Cookies.Select(c => c.Name).Contains("XAM_AUTH"));
            Assert.True(token.Cookies.Select(c => c.Name).Contains("xinssid"));
        }
    }
}
