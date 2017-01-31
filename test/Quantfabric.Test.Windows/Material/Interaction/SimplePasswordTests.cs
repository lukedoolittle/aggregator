using System.Linq;
using Material.Infrastructure.ProtectedResources;
using Material.OAuth.Workflow;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    public class SimplePasswordTests
    {
        [Fact]
        public async void CanGetValidCredentialsFromXamarinInsights()
        {
            var username = "";
            var password = "";

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
