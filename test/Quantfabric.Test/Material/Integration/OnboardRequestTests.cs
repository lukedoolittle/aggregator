#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using Material;
using Aggregator.Framework;
using Aggregator.Infrastructure.Clients;
using Aggregator.Infrastructure.Adapters;
using Aggregator.Test.Helpers.Fixtures;
using Foundations.Serialization;
using Material.Infrastructure.Static;
using Newtonsoft.Json.Linq;
using Xunit;
#if __ANDROID__
using Android.App;
#endif

namespace Quantfabric.Test.Material.Integration
{
    public class OnboardRequestTests
    {
        [Fact]
        public async void ConnectToAlphaMioAndCollectValues()
        {
            //TODO: need to put credentials in here
            var myResult = await Requester.MakeBluetoothRequest<MioHeartRate>(null)
                .ConfigureAwait(false);
            var results = myResult.AsEntity<List<Tuple<DateTimeOffset, JObject>>>();

            Assert.Equal(1, results.Count());
            var result = results.Single();
            var heartRateValue = Convert.ToInt32(result.Item2["heartRate"].ToString());
            Assert.True(40 <= heartRateValue);
            Assert.True(200 >= heartRateValue);
        }

        [Fact]
        public async void GetGPSCoordinates()
        {
            var myResult = await Requester.MakeGPSRequest().ConfigureAwait(false);
            var results = myResult.AsEntity<List<Tuple<DateTimeOffset, JObject>>>();

            Assert.True(results.Count() == 1);

            var result = results.Single();
            Assert.NotNull(result.Item2["latitude"]);
            Assert.NotNull(result.Item2["longitude"]);
            Assert.NotNull(result.Item2["speed"]);
            Assert.NotNull(result.Item2["timestamp"]);
        }
#if __ANDROID__
        [Fact]
        public async System.Threading.Tasks.Task MakeSMSRequestWithFilter()
        {
            var request = new SMSClient(new AndroidSMSAdapter());

            //fill in a real date here
            var date = 1454703049063;
            //var date = DateTime.Now.ToUnixTimeSeconds();

            //IF USING EMULATOR
            //open a console
            //connect via telnet to the running emulator: telnet localhost 5554(you can find the portnumber in the title of the emulator)
            //type this: sms send senderPhoneNumber textmessage

            //IF USING PHYSICAL DEVICE
            //send yourself a text message now
            var resultsTask = request.GetDataPoints(date.ToString());

            var results = await resultsTask;

            Assert.True(results.Count() == 1);

            var result = results.Single();
            Assert.NotNull(result.Item2["address"]);
            Assert.NotNull(result.Item2["body"]);
            Assert.NotNull(result.Item2["date"]);
        }


        //IF USING EMULATOR
        //open a console
        //connect via telnet to the running emulator: telnet localhost 5554(you can find the portnumber in the title of the emulator)
        //type this: sms send senderPhoneNumber textmessage
        //do this as many times as you want to see text messages here

        //IF USING PHYSICAL DEVICE
        //ensure that you have some text messages on your phone
        [Fact]
        public async System.Threading.Tasks.Task MakeAllSMSRequest()
        {
            var request = new SMSClient(new AndroidSMSAdapter());

            var resultsTask = request.GetDataPoints(null);

            var results = await resultsTask;

            Assert.True(results.Count() > 0);

            foreach (var result in results)
            {
                Assert.NotNull(result.Item2["address"]);
                Assert.NotNull(result.Item2["body"]);
                Assert.NotNull(result.Item2["date"]);
            }
        }
#endif
    }
}
#endif
