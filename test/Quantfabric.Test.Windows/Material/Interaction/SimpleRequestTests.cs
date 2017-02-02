using System;
using System.IO;
using Foundations.Enums;
using Material.Contracts;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Material.OAuth.Workflow;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    [Trait("Category", "Continuous")]
    public class SimpleRequestTests
    {
        private readonly AppCredentialRepository _appRepository
            = new AppCredentialRepository(CallbackType.Localhost);

        [Fact]
        public async void MakeRequestForLanguageUnderstandingServicePrediction()
        {
            var credentials = _appRepository.GetApiKeyCredentials<MicrosoftLuis>();
            var actualText = "set a timer for 5 minutes";
            var request = new MicrosoftLuisPredict
            {
                AppId = "2650ce1d-c2f7-4ea4-afaa-ccbc0104adb1",
                Example = actualText
            };
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<MicrosoftLuisPredict, MicrosoftLuisPredictResponse>(request)
                .ConfigureAwait(false);

            Assert.Equal(actualText, response.UtteranceText);
            Assert.NotNull(response.EntitiesResults);
            Assert.NotNull(response.IntentsResults);
        }

        [Fact]
        public async void MakeRequestForSpeechToText()
        {
            MicrosoftBingSpeechToText.DeviceId = Guid.NewGuid();

            var apiKeyCredentials = _appRepository.GetApiKeyCredentials<MicrosoftBing>();
            var credentials = await new ApiKeyAssert<MicrosoftBing>(apiKeyCredentials.KeyValue)
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            var actualText = "hi i'm brian";

            var request = new MicrosoftBingSpeechToText
            {
                Body = File.OpenRead("brian.wav"),
                BodyType = MediaType.Wave
            };

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<MicrosoftBingSpeechToText, MicrosoftBingSpeechResponse>(request)
                .ConfigureAwait(false);

            Assert.True(response.Results[0].Lexical.StartsWith(actualText));
        }
    }
}
