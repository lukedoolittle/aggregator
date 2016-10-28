using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    public class SimpleRequestTests
    {
        private readonly AppCredentialRepository _appRepository
            = new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void MakeRequestForLanguageUnderstandingServicePrediction()
        {
            var credentials = _appRepository.GetApiKeyCredentials<MicrosoftLuis>();
            var actualText = "Set a timer for 5 minutes".ToLower();
            var request = new MicrosoftLuisPredict
            {
                AppId = "2650ce1d-c2f7-4ea4-afaa-ccbc0104adb1",
                Example = actualText
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<MicrosoftLuisPredict, MicrosoftLuisPredictResponse>(request)
                .ConfigureAwait(false);

            Assert.Equal(actualText, response.UtteranceText);
            Assert.NotNull(response.EntitiesResults);
            Assert.NotNull(response.IntentsResults);
        }
    }
}
