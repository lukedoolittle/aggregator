using System;
using System.IO;
using Material.Application;
using Material.Contracts;
using Material.Domain.Responses;
using Material.Framework.Enums;
using Material.Domain.Requests;
using Material.Domain.ResourceProviders;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Quantfabric.Test.Material.Integration
{
    [Trait("Category", "Continuous")]
    public class SimpleRequestTests
    {
        private readonly AppCredentialRepository _appRepository
            = new AppCredentialRepository(CallbackType.Localhost);

        #region Yesmail Requests

        [Fact]
        public async void MakeRequestForYesmailReportData()
        {
            var credentials = _appRepository.GetApiKeyCredentials<Yesmail>();

            var request = new YesmailReferenceDataRecords()
            {
                ApiUser = "musicnotes",
                Dataset = "powerbi",
                Offset = 0,
                Limit = 100
            }
            .UseTestEnvironment();

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<YesmailReferenceDataRecords>(request)
                .ConfigureAwait(false);

            var result = await response.ContentAsync().ConfigureAwait(false);

            Assert.NotEmpty(result);
        }

        #endregion Yesmail Requests

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

            var request = new MicrosoftBingSpeechToText();
            request.AddContent(File.OpenRead("brian.wav"), MediaType.Wave);

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<MicrosoftBingSpeechToText, MicrosoftBingSpeechResponse>(request)
                .ConfigureAwait(false);

            Assert.True(response.Results[0].Lexical.StartsWith(actualText));
        }
    }
}
