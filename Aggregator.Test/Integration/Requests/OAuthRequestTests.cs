using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Infrastructure.Requests;
using Aggregator.Test.Helpers.Fixtures;
using Xunit;

namespace Aggregator.Test.Integration
{
    using Aggregator.Infrastructure.Services;


    public class OAuthRequestTests : IClassFixture<OAuthRequestFixture>
    {
        private readonly OAuthRequestFixture _fixture;

        public OAuthRequestTests(OAuthRequestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void MakeRequestForTwitterTweets()
        {
            var result = 
                await _fixture.MakeRequestForService<Twitter, TwitterTweet>();
            AssertDataPoints(result);
        }

        [Fact(Skip = "Restsharp bug similar to fatsecret problem")]
        public async void MakeRequestForWithingsWeighins()
        {
            var result =
                await _fixture.MakeRequestForService<Withings, WithingsWeighin>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForGmailEmails()
        {
            var result =
                await _fixture.MakeRequestForService<Google, GoogleGmail>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForRescuetimeAnalyticData()
        {
            var result =
                await _fixture.MakeRequestForService<Rescuetime, RescuetimeAnalyticData>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFitbitIntradaySteps()
        {
            var result =
                await _fixture.MakeRequestForService<Fitbit, FitbitIntradaySteps>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterRetweetsOfMe()
        {
            var result =
                await _fixture.MakeRequestForService<Twitter, TwitterRetweetOfMe>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterDirectMessage()
        {
            var result =
                await _fixture.MakeRequestForService<Twitter, TwitterSentDirectMessage>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterFollowing()
        {
            var result =
                await _fixture.MakeRequestForService<Twitter, TwitterFollowers>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterFollowers()
        {
            var result = 
                await _fixture.MakeRequestForService<Twitter, TwitterFollowers>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterFavorite()
        {
            var result = 
                await _fixture.MakeRequestForService<Twitter, TwitterFavorite>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterMention()
        {
            var result = 
                await _fixture.MakeRequestForService<Twitter, TwitterMention>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterTimeline()
        {
            var result = 
                await _fixture.MakeRequestForService<Twitter, TwitterTimeline>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFoursquareCheckins()
        {
            var result = 
                await _fixture.MakeRequestForService<Foursquare, FoursquareCheckin>();
            AssertDataPoints(result);
        }

        [Fact(Skip="There is no timeseries data here")]
        public async void MakeRequestForFoursquareFriends()
        {
            var result = 
                await _fixture.MakeRequestForService<Foursquare, FoursquareFriend>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFoursquareTip()
        {
            var result = 
                await _fixture.MakeRequestForService<Foursquare, FoursquareTip>();
            AssertDataPoints(result);

        }

        [Fact]
        public async void MakeRequestForSpotify()
        {
            var result = 
                await _fixture.MakeRequestForService<Spotify, SpotifySavedTracks>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookFeed()
        {
            var result = 
                await _fixture.MakeRequestForService<Facebook,FacebookFeed>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookPageLike()
        {
            var result = 
                await _fixture.MakeRequestForService <Facebook,FacebookPageLike>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookActivity()
        {
            var result = 
                await _fixture.MakeRequestForService <Facebook,FacebookActivity>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookEvent()
        {
            var result = 
                await _fixture.MakeRequestForService <Facebook,FacebookEvent>();
            AssertDataPoints(result);
        }

        [Fact(Skip = "No timeseries data")]
        public async void MakeRequestForFacebookFriend()
        {
            var result = 
                await _fixture.MakeRequestForService <Facebook,FacebookFriend>();
            AssertDataPoints(result);
        }

        [Fact(Skip = "Need parter API account")]
        public async void MakeRequestForLinkedInUpdate()
        {
            var result = 
                await _fixture.MakeRequestForService<Linkedin, LinkedinUpdate>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForRunkeeperFitnessActivities()
        {
            var result = 
                await _fixture.MakeRequestForService<Runkeeper, RunkeeperFitnessActivities>();
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFitbitSleep()
        {
            var result = 
                await _fixture.MakeRequestForService<Fitbit, FitbitSleep>();
            AssertDataPoints(result);
        }

        [Fact(Skip = "Broken fatsecret OAuth1 integration")]
        public async void MakeRequestForFatsecretMeals()
        {
            var result = 
                await _fixture.MakeRequestForService<Fatsecret, FatsecretMeal>();
            AssertDataPoints(result);
        }

        private void AssertDataPoints(IEnumerable<Tuple<DateTimeOffset, JObject>> actual)
        {
            Assert.NotNull(actual);

            var dataPoints = actual as Tuple<DateTimeOffset, JObject>[] ?? actual.ToArray();
            if (!dataPoints.Any())
            {
                //Should do some sort of inconclusive thing here
            }

            foreach (var item in dataPoints)
            {
                Assert.NotNull(item);
                if (item.Item1 != DateTimeOffset.MinValue)
                {
                    Assert.True(item.Item1 > _fixture.MinimumSampleDatetime);
                    //Assert.True(item.Item1 < _fixture.MaximumSampleDatetime);
                }
            }
        }
    }
}
