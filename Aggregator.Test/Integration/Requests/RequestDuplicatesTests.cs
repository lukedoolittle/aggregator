using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Mail;
using BatmansBelt;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Domain.Write.Samples;
using Aggregator.Framework;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Clients;
using Aggregator.Infrastructure.Requests;
using Aggregator.Task.Commands;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Fixtures;
using Aggregator.Test.Helpers.Mocks;
using SimpleCQRS.Framework.Contracts;
using Xunit;

namespace Aggregator.Test.Integration
{
    using Infrastructure.Services;
    using System.Threading.Tasks;

    public class RequestDuplicatesTests : IClassFixture<OAuthRequestFixture>
    {
        private readonly OAuthRequestFixture _fixture;

        public RequestDuplicatesTests(OAuthRequestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void MakeTwoRunkeeperRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Runkeeper, RunkeeperFitnessActivities>(
                DuplicateResponseEnum.Samples);
        }

        [Fact]
        public async void MakeTwoFoursquareRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Foursquare, FoursquareCheckin>(
                DuplicateResponseEnum.NoSamples);
        }

        [Fact]
        public async void MakeTwoRescuetimeRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Rescuetime, RescuetimeAnalyticData>(
                DuplicateResponseEnum.LessSamples);
        }

        [Fact]
        public async void MakeTwoTwitterTweetRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Twitter, TwitterTweet>(
                DuplicateResponseEnum.NoSamples);
        }

        [Fact]
        public async void MakeTwoFitbitSleepRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Fitbit, FitbitSleep>(
                DuplicateResponseEnum.LessSamples);
        }

        [Fact]
        public async void MakeTwoFitbitStepRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Fitbit, FitbitIntradaySteps>(
                DuplicateResponseEnum.LessSamples);
        }

        [Fact]
        public async void MakeTwoFacebookFeedRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Facebook, FacebookFeed>(
                DuplicateResponseEnum.NoSamples);
        }

        [Fact]
        public async void MakeTwoGmailRequestsExpectNoNewDataPointsOnSecondRequest()
        {
            await MakeDuplicateRequests<Google, GoogleGmail>(
                DuplicateResponseEnum.Samples);
        }

        private async Task AddSamples<TRequest>(
            IEnumerable<Tuple<DateTimeOffset, JObject>> samples,
            IEventHandler<SampleAdded<TRequest>> eventHandler,
            Person person)
            where TRequest : Request, new()
        {
            foreach (var sample in samples)
            {
                var hash = Hashing.CreateGuidFromData(sample.Item2.ToString());
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


        private async Task MakeDuplicateRequests<TService, TRequest>(
            DuplicateResponseEnum responseType)
            where TRequest : Request, new()
            where TService : Service, new()
        {
            var databaseMock = new DatabaseMock<SampleDto<TRequest>>();
            var sampleAddedEventHandler = new SampleAddedEventHandler<TRequest>(databaseMock);

            var aggregateId = Guid.NewGuid();
            var root = new Person(aggregateId);
            root.CreateToken<TService>(new JObject());
            root.AddSensor<TRequest>(PollingIntervalEnum.Moderate, true);
            root.MarkChangesAsCommitted();

            var samples = await _fixture.MakeRequestForService<TService, TRequest>();
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

            samples = await _fixture.MakeRequestForService<TService, TRequest>(recency);

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
            root.AddSensor<GoogleGmail>(PollingIntervalEnum.Moderate, true);
            root.MarkChangesAsCommitted();

            var samples = await _fixture.MakeRequestForService<Google, GoogleGmail>();
            await AddSamples(samples, sampleAddedEventHandler, root);
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
            samples = await _fixture.MakeRequestForService<Google, GoogleGmail>();
            await AddSamples(samples, sampleAddedEventHandler, root);

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
            root.AddSensor<GoogleGmail>(PollingIntervalEnum.Moderate, true);
            root.MarkChangesAsCommitted();

            var client = new SMSClient(new Infrastructure.ComponentManagers.AndroidSMSManager());
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

        private enum DuplicateResponseEnum
        {
            NoSamples,
            LessSamples,
            Samples
        }
    }
}
