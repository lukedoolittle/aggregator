using System;
using Material;
using Material.Infrastructure.Responses;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Xunit;
using Fitbit = Material.Infrastructure.ProtectedResources.Fitbit;
using Google = Material.Infrastructure.ProtectedResources.Google;
using GoogleGmailMetadata = Material.Infrastructure.Requests.GoogleGmailMetadata;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;
using WithingsWeighin = Material.Infrastructure.Requests.WithingsWeighin;

namespace Quantfabric.Test.Material.Integration
{
    public class OAuthSimpleRequestTests
    {
        [Fact]
        public async void MakeRequestForLinkedInPersonal()
        {
            var credentials = TestSettings.GetToken<LinkedIn, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new LinkedinPersonal();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<LinkedinPersonal, LinkedInPersonalResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFatsecretMeals()
        {
            var credentials = TestSettings.GetToken<Fatsecret, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FatsecretMeal
            {
                Date = DateTime.Today
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FatsecretMeal, FatsecretMealResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForWithingsWeighins()
        {
            var credentials = TestSettings.GetToken<Withings, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new WithingsWeighin()
            {
                Lastupdate = DateTime.Today.Subtract(TimeSpan.FromDays(50))
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<WithingsWeighin, WithingsWeighInResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForGmailMetadata()
        {
            var credentials = TestSettings.GetToken<Google, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new GoogleGmailMetadata
            {
                After = DateTime.Today.Subtract(TimeSpan.FromDays(2)),
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<GoogleGmailMetadata, GoogleGmailMetadataResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForGmails()
        {
            var credentials = TestSettings.GetToken<Google, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var ids =
                "155eb306d5906267,155d77ab8bd5ff83,155d6e3d49a8851e,155d0cb7ecef940a,155c75be7cf36899,155c60466b49fb9c,155c3586947e6a40,155bd9817b81a36e,155bd98091e6e545,155ac8385b14f3b0,155ac8378c283efa"
                    .Split(',');

            foreach (var id in ids)
            {
                var request = new GoogleGmail {MessageId = id};
                var response = await new OAuthRequester(credentials)
                    .MakeOAuthRequestAsync<GoogleGmail, GoogleGmailResponse>(request).ConfigureAwait(false);

                Assert.NotNull(response);
            }
        }

        [Fact]
        public async void MakeRequestForRescuetimeAnalyticData()
        {
            var credentials = TestSettings.GetToken<Rescuetime, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new RescuetimeAnalyticData();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<RescuetimeAnalyticData, RescuetimeAnalyticDataResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForRescuetimeAnalyticDataWithStartAndEndTimes()
        {
            var credentials = TestSettings.GetToken<Rescuetime, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new RescuetimeAnalyticData
            {
                RestrictBegin = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
                RestrictEnd = DateTime.Now.Subtract(TimeSpan.FromDays(1))
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<RescuetimeAnalyticData, RescuetimeAnalyticDataResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForRunkeeperFitnessActivities()
        {
            var credentials = TestSettings.GetToken<Runkeeper, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new RunkeeperFitnessActivity()
            {
                NoEarlierThan = DateTime.Today.Subtract(TimeSpan.FromDays(60))
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<RunkeeperFitnessActivity, RunkeeperFitnessActivityResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwentyThreeAndMeUser()
        {
            var credentials = TestSettings.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwentyThreeAndMeUser();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwentyThreeAndMeUser, TwentyThreeAndMeUserResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact(Skip = "Don't have a genome sequenced")]
        public async void MakeRequestForTwentyThreeAndMeGenome()
        {
            var credentials = TestSettings.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwentyThreeAndMeGenome
            {
                ProfileId = "some profile id with a genome"
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwentyThreeAndMeGenome, TwentyThreeAndMeGenomeResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForSpotifyPlayList()
        {
            var credentials = TestSettings.GetToken<Spotify, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new SpotifySavedTrack();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<SpotifySavedTrack, SpotifySavedTrackResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFoursquareCheckins()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FoursquareCheckin();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FoursquareCheckin, FoursquareCheckinResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFoursquareCheckinsWithTimeRange()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FoursquareCheckin
            {
                AfterTimestamp = DateTime.Today.Subtract(TimeSpan.FromDays(90)),
                BeforeTimestamp = DateTime.Today.Subtract(TimeSpan.FromDays(30))
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FoursquareCheckin, FoursquareCheckinResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFoursquareFriends()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FoursquareFriend();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FoursquareFriend, FoursquareFriendResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFoursquareTip()
        {
            var credentials = TestSettings.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FoursquareTip();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FoursquareTip, FoursquareTipResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFitbitIntradaySteps()
        {
            var credentials = TestSettings.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitIntradaySteps
            {
                Startdate = DateTime.Today,
                Enddate = DateTime.Today,
                Starttime = DateTime.Now.Subtract(TimeSpan.FromHours(5)),
                Endtime = DateTime.Now
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitIntradaySteps, FitbitIntradayStepsResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFitbitIntradayStepsBulk()
        {
            var credentials = TestSettings.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitIntradayStepsBulk
            {
                Date = DateTime.Today.Subtract(TimeSpan.FromDays(2))
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitIntradayStepsBulk, FitbitIntradayStepsResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact(Skip = "Don't have heart rate data for myself")]
        public async void MakeRequestForFitbitIntradayHeartRate()
        {
            var credentials = TestSettings.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitIntradayHeartRate()
            {
                Startdate = DateTime.Today,
                Enddate = DateTime.Today,
                Starttime = DateTime.Now.Subtract(TimeSpan.FromHours(5)),
                Endtime = DateTime.Now
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitIntradayHeartRate, string>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact(Skip = "Don't have heart rate data for myself")]
        public async void MakeRequestForFitbitIntradayHeartRateBulk()
        {
            var credentials = TestSettings.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitIntradayHeartRateBulk()
            {
                Date = DateTime.Today.Subtract(TimeSpan.FromDays(2))
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitIntradayHeartRateBulk, string>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFitbitSleep()
        {
            var credentials = TestSettings.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitSleep
            {
                Date = DateTime.Today
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitSleep, FitbitSleepResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFitbitProfile()
        {
            var credentials = TestSettings.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitProfile();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitProfile, FitbitProfileResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFacebookLike()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FacebookPageLike()
            {
                Since = DateTime.Today.Subtract(TimeSpan.FromDays(200)),
                Until = DateTime.Today,
                Limit = 3
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FacebookPageLike, FacebookPageLikeResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFacebookEvent()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FacebookEvent()
            {
                Since = DateTime.Today.Subtract(TimeSpan.FromDays(500)),
                Until = DateTime.Today,
                Limit = 3
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FacebookEvent, FacebookEventResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForFacebookFeed()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FacebookFeed()
            {
                Since = DateTime.Today.Subtract(TimeSpan.FromDays(500)),
                Until = DateTime.Today,
                Limit = 10
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FacebookFeed, FacebookFeedResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact(Skip = "Don't have any facebook friends with access to app")]
        public async void MakeRequestForFacebookFriend()
        {
            var credentials = TestSettings.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FacebookFriend()
            {
                Since = DateTime.Today.Subtract(TimeSpan.FromDays(500)),
                Until = DateTime.Today,
                Limit = 10
            };
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FacebookFriend, string>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterTweets()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterTweet();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterTweet, TwitterTweetResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterTimeline()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterTimeline();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterTimeline, TwitterTimelineResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterMentions()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterMention();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterMention, TwitterMentionResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterFavorites()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterFavorite();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterFavorite, TwitterFavoriteResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterFollowers()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterFollower();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterFollower, TwitterFollowerResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterFollowing()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterFollowing();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterFollowing, TwitterFollowingResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact(Skip = "Dont have any twitter direct messages")]
        public async void MakeRequestForTwitterReceivedDirectMessage()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterReceivedDirectMessage();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterReceivedDirectMessage, string>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact(Skip = "Dont have any twitter direct messages")]
        public async void MakeRequestForTwitterSentDirectMessage()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterSentDirectMessage();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterSentDirectMessage, string>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForTwitterRetweet()
        {
            var credentials = TestSettings.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterRetweetOfMe();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterRetweetOfMe, TwitterRetweetOfMeResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }
    }
}
