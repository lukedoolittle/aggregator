using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Domain.Write.Samples;
using Aggregator.Task.EventHandlers;
using Aggregator.Task.Factories;
using Aggregator.Test.Helpers.Mocks;
using Foundations.Cryptography;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Task;
using SimpleCQRS.Framework.Contracts;
using Xunit;
using Fitbit = Material.Infrastructure.ProtectedResources.Fitbit;
using Google = Material.Infrastructure.ProtectedResources.Google;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;
#if __MOBILE__

#endif


namespace Quantfabric.Test.Material.Integration
{
    using System.Threading.Tasks;

    public class RequestDuplicatesTests 
    {
        [Fact]
        public async void MakeTwoRunkeeperRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Runkeeper, RunkeeperFitnessActivity, OAuth2Credentials>(
                    DuplicateResponseEnum.Samples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoFoursquareRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Foursquare, FoursquareCheckin, OAuth2Credentials>(
                    DuplicateResponseEnum.NoSamples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoRescuetimeRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Rescuetime, RescuetimeAnalyticData, OAuth2Credentials>(
                    DuplicateResponseEnum.LessSamples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoTwitterTweetRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Twitter, TwitterTweet, OAuth1Credentials>(
                    DuplicateResponseEnum.NoSamples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoFitbitSleepRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Fitbit, FitbitSleep, OAuth2Credentials>(
                    DuplicateResponseEnum.LessSamples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoFitbitStepRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Fitbit, FitbitIntradaySteps, OAuth2Credentials>(
                    DuplicateResponseEnum.LessSamples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoFacebookFeedRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Facebook, FacebookFeed, OAuth2Credentials>(
                    DuplicateResponseEnum.NoSamples)
                .ConfigureAwait(false); ;
        }

        [Fact]
        public async void MakeTwoGmailRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Google, GoogleGmail, OAuth2Credentials>(
                    DuplicateResponseEnum.Samples)
                .ConfigureAwait(false);
        }

        private async Task AddSamples<TRequest>(
            IEnumerable<Tuple<DateTimeOffset, JObject>> samples,
            IEventHandler<SampleAdded<TRequest>> eventHandler,
            Person person)
            where TRequest : Request, new()
        {
            foreach (var sample in samples)
            {
                var hash = Security.CreateGuidFromData(sample.Item2.ToString());
                var @event = new SampleAdded<TRequest>(
                    sample.Item2,
                    sample.Item1,
                    hash)
                {
                    AggregateId = person.Id,
                    Version = 0
                };
                eventHandler.Handle(@event);
            }

            if (samples.Any() &&
                !string.IsNullOrEmpty(new TRequest().ResponseFilterKey))
            {
                person.ChangeFilter<TRequest>(samples);
            }
        }


        private async Task MakeDuplicateRequests<TService, TRequest, TCredentials>(
            DuplicateResponseEnum responseType)
            where TRequest : OAuthRequest, new()
            where TService : ResourceProvider, new()
            where TCredentials : TokenCredentials
        {
            var databaseMock = new DatabaseMock<SampleDto<TRequest>>();
            var sampleAddedEventHandler = new SampleAddedEventHandler<TRequest>(databaseMock);

            var aggregateId = Guid.NewGuid();
            var root = new Person(aggregateId);
            root.CreateToken<TService>(new JObject());
            root.AddSensor<TRequest>(TimeSpan.Zero, true);
            root.MarkChangesAsCommitted();

            var credentials = TestSettings.GetToken<TService, TCredentials>();
            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }
            var samples = await MakeTimeseriesRequest<TRequest>(credentials).ConfigureAwait(false);

            var initialSampleCount = samples.Count();
            await AddSamples(samples, sampleAddedEventHandler, root);

            var items = databaseMock.GetAll(aggregateId.ToString());
            Assert.True(items.Any());

            var recencyEvent =
                root.GetUncommittedChanges().SingleOrDefault(c => c is FilterChanged<TRequest>) as
                    FilterChanged<TRequest>;
            if (recencyEvent == null)
            {
                throw new Exception("No recency event created");
            }

            var recency = recencyEvent.NewFilter;

            root.MarkChangesAsCommitted();

            samples = await MakeTimeseriesRequest<TRequest>(
                    credentials, 
                    recency)
                .ConfigureAwait(false);

            if (responseType == DuplicateResponseEnum.NoSamples)
            {
                Assert.True(samples.Count() <= 1);
            }
            else if (responseType == DuplicateResponseEnum.LessSamples)
            {
                Assert.True(initialSampleCount > samples.Count());
            }

            await AddSamples(samples, sampleAddedEventHandler, root);
       
            Assert.Equal(items, databaseMock.GetAll(aggregateId.ToString()));
        }


        [Fact(Skip = "Enter email address")]
        public async void MakeGmailRequestThenSendEmailAndMakeAnotherRequestExpectNewDataPointsOnSecondRequest()
        {
            var databaseMock = new DatabaseMock<SampleDto<GoogleGmail>>();
            var sampleAddedEventHandler = new SampleAddedEventHandler<GoogleGmail>(databaseMock);

            var aggregateId = Guid.NewGuid();
            var root = new Person(aggregateId);
            root.CreateToken<Google>(new JObject());
            root.AddSensor<GoogleGmail>(TimeSpan.Zero, true);
            root.MarkChangesAsCommitted();

            var credentials = TestSettings.GetToken<Google, OAuth2Credentials>();
            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }
            var samples = await MakeTimeseriesRequest<GoogleGmail>(credentials).ConfigureAwait(false);

            await AddSamples(samples, sampleAddedEventHandler, root).ConfigureAwait(false);
            root.MarkChangesAsCommitted();

            var items = databaseMock.GetAll(aggregateId.ToString());
            Assert.True(items.Any());

            MailMessage mail = new MailMessage(
                "",
                "")
            {
                Subject = "this is a test email.",
                Body = "this is my test email body"
            };

            SmtpClient client = new SmtpClient
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.google.com"
            };
            client.Send(mail);
            samples = await MakeTimeseriesRequest<GoogleGmail>(credentials).ConfigureAwait(false);
            await AddSamples(samples, sampleAddedEventHandler, root).ConfigureAwait(false);

            Assert.Equal(items, databaseMock.GetAll(aggregateId.ToString()));
        }

        //TODO: this is wrong shouldn't be Google should be SMS
#if __ANDROID__
        [Fact]
        public async void MakeTwoSMSRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            var databaseMock = new DatabaseMock<SampleDto<GoogleGmail>>();
            var sampleAddedEventHandler = new SampleAddedEventHandler<GoogleGmail>(databaseMock);

            var aggregateId = Guid.NewGuid();
            var root = new Person(aggregateId);
            root.CreateToken<Google>(new JObject());
            root.AddSensor<GoogleGmail>(TimeSpan.Zero, true);
            root.MarkChangesAsCommitted();

            var client = new SMSClient(new AndroidSMSAdapter());
            var samples = await client.GetDataPoints(null);
            await AddSamples(samples, sampleAddedEventHandler, root);
            var items = databaseMock.GetAll(aggregateId.ToString());
            Assert.True(items.Any());
            root.MarkChangesAsCommitted();

            samples = await client.GetDataPoints("something here");

            //here you will have to send a text message as you do in the other test
            await AddSamples(samples, sampleAddedEventHandler, root);
            Assert.Equal(items, databaseMock.GetAll(aggregateId.ToString()));
            Assert.True(root.GetUncommittedChanges().Count() == 1);
        }
#endif

        private async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> MakeTimeseriesRequest<TRequest>(
            TokenCredentials credentials,
            string recencyValue = null)
            where TRequest : Request
        {
            var clientFactory = new ClientFactory(new OAuthFactory());

            var client = clientFactory.CreateClient<TRequest, TokenCredentials>(credentials);

            var result = await client
                .GetDataPoints(recencyValue)
                .ConfigureAwait(false);

            return result;
        }
        private enum DuplicateResponseEnum
        {
            NoSamples,
            LessSamples,
            Samples
        }
    }
}
