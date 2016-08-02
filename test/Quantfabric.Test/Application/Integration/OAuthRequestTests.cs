using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Material;
using Material.Infrastructure.Responses;
using Newtonsoft.Json.Linq;
using Aggregator.Task.Factories;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Task;
using Xunit;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;
using WithingsWeighin = Material.Infrastructure.Requests.WithingsWeighin;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuthRequestTests
    {
        private readonly DateTime _minimumSampleDatetime = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 10));
        private readonly DateTime _maximumSampleDatetime = DateTime.Now;

        [Fact]
        public async void MakeRequestForTwitterTweets()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) {throw new Exception("Expired credentials!!!");}

            var result = await MakeTimeseriesRequest<TwitterTweet>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForWithingsWeighins()
        {
            var credentials = TestSettings.GetToken<Withings, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<WithingsWeighin>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForGmailEmails()
        {
            var credentials = TestSettings.GetToken<global::Material.Infrastructure.ProtectedResources.Google, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<GoogleGmail>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForRescuetimeAnalyticData()
        {
            var credentials = TestSettings.GetToken<Rescuetime, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<RescuetimeAnalyticData>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFitbitIntradaySteps()
        {
            var credentials = TestSettings.GetToken<global::Material.Infrastructure.ProtectedResources.Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FitbitIntradaySteps>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterRetweetsOfMe()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterRetweetOfMe>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterDirectMessage()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterReceivedDirectMessage>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterFollowing()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterFollowing>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterFollowers()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterFollower>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterFavorite()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterFavorite>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterMention()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterMention>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwitterTimeline()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<TwitterTimeline>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFoursquareCheckins()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FoursquareCheckin>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact(Skip="There is no timeseries data here")]
        public async void MakeRequestForFoursquareFriends()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FoursquareFriend>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFoursquareTip()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FoursquareTip>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);

        }

        [Fact]
        public async void MakeRequestForSpotify()
        {
            var credentials = TestSettings.GetToken<Spotify, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<SpotifySavedTrack>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookFeed()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FacebookFeed>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookPageLike()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FacebookPageLike>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFacebookEvent()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FacebookEvent>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact(Skip = "No timeseries data")]
        public async void MakeRequestForFacebookFriend()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FacebookFriend>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForRunkeeperFitnessActivities()
        {
            var credentials = TestSettings.GetToken<Runkeeper, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<RunkeeperFitnessActivity>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForFitbitSleep()
        {
            var credentials = TestSettings.GetToken<global::Material.Infrastructure.ProtectedResources.Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FitbitSleep>(credentials).ConfigureAwait(false);

            AssertDataPoints(result);
        }

        [Fact(Skip = "Broken fatsecret OAuth1 integration")]
        public async void MakeRequestForFatsecretMeals()
        {
            var credentials = TestSettings.GetToken<Fatsecret, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await MakeTimeseriesRequest<FatsecretMeal>(credentials).ConfigureAwait(false);
            AssertDataPoints(result);
        }

        [Fact]
        public async void MakeRequestForTwentyThreeAndMeProfile()
        {
            var credentials = TestSettings.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await new OAuthRequester(credentials).MakeOAuthRequestAsync<TwentyThreeAndMeUser, TwentyThreeAndMeUserResponse>().ConfigureAwait(false);
        }

        [Fact]
        public async void MakeRequestForTwentyThreeAndMeGenome()
        {
            var credentials = TestSettings.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var result = await new OAuthRequester(credentials).MakeOAuthRequestAsync<TwentyThreeAndMeGenome, TwentyThreeAndMeGenomeResponse>().ConfigureAwait(false);
        }

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
                    Assert.True(item.Item1 > _minimumSampleDatetime);
                    //Had to remove this because there are some timeseries points "from the future"
                    //including Facebook events
                    //Assert.True(item.Item1 < _fixture.MaximumSampleDatetime);
                }
            }
        }
    }
}
