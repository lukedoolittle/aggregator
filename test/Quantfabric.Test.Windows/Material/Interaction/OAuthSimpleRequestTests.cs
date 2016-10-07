using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.HttpClient.Serialization;
using Material;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.RequestBodies;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    public class OAuthSimpleRequestTests
    {
        private readonly TokenCredentialRepository _tokenRepository
            = new TokenCredentialRepository(true);
        private readonly AppCredentialRepository _appRepository
            = new AppCredentialRepository(CallbackTypeEnum.Localhost);

        #region Linkedin Requests

        [Fact]
        public async void MakeRequestForLinkedInPersonal()
        {
            var credentials = _tokenRepository.GetToken<LinkedIn, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new LinkedinPersonal();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<LinkedinPersonal, LinkedInPersonalResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion LinkedinRequests

        #region Fatsecret Requests

        [Fact]
        public async void MakeRequestForFatsecretMeals()
        {
            var credentials = _tokenRepository.GetToken<Fatsecret, OAuth1Credentials>();

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

        #endregion Fatsecret Requests

        public void MakeRequestForOmnitureUserLevelReports()
        {
            var body = new OmnitureQueueBody
            {
                ReportDescription = new OmnitureReportDescription
                {
                    DateGranularity = OmnitureReportingDateGranularityEnum.Hour,
                    ReportSuiteId = "musicnotes",
                    Date = new DateTime(2016, 6, 1),
                    Metrics = new List<OmnitureMetric> {new OmnitureMetric {Id = "pageviews"}},
                    Elements = new List<OmnitureElement>
                    {
                        new OmnitureElement
                        {
                            Id = "eVar39",
                            Selected = new List<string>
                            {
                                ""
                            }
                        },
                        new OmnitureElement
                        {
                            Id = "page"
                        }
                    }
                }
            };

            throw new NotImplementedException();
        }

        [Fact]
        public async void MakeRequestForOmnitureAggregateReports()
        {
            var credentials = _tokenRepository.GetToken<Omniture, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var body = new OmnitureQueueBody
            {
                ReportDescription = new OmnitureReportDescription
                {
                    DateGranularity = OmnitureReportingDateGranularityEnum.Hour,
                    ReportSuiteId = "musicnotes",
                    StartDate = new DateTime(2015, 10, 6),
                    EndDate = new DateTime(2016, 10, 6),
                    Metrics = new List<OmnitureMetric> { new OmnitureMetric { Id = "visits" } }
                }
            };

            var request = new OmnitureReporting
            {
                Method = OmnitureReportingMethodEnum.ReportQueue,
                Body = body
            };

            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<OmnitureReporting, OmnitureQueueResponse>(request)
                .ConfigureAwait(false);

            System.Threading.Thread.Sleep(10000);

            var getBody = new OmnitureGetBody
            {
                ReportId = response.ReportId
            };

            request = new OmnitureReporting
            {
                Method = OmnitureReportingMethodEnum.ReportGet,
                Body = getBody
            };

            var getResponse = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<OmnitureReporting, OmnitureGetResponse>(request)
                .ConfigureAwait(false);

            //var text = System.IO.File.ReadAllText("results.json");
            //var serializer = new JsonSerializer();
            //var result = serializer.Deserialize<OmnitureGetResponse>(text);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"lines.csv"))
            {
                foreach (var item in getResponse.Report.Data)
                {
                    var date = $"{item.Year}-{item.Month}-{item.Day}-{item.Hour}:00";
                    file.WriteLine($"{date},{item.Counts.First()}");
                }
            }

            Assert.NotNull(getResponse.Report.Totals[0]);
        }

        [Fact]
        public async void MakeRequestForWithingsWeighins()
        {
            var credentials = _tokenRepository.GetToken<Withings, OAuth1Credentials>();

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

        #region Google Requests

        [Fact]
        public async void MakeRequestForGoogleAnalyticsReports()
        {
            var privateKey = _appRepository.GetPrivateKey<GoogleAnalytics>();
            var clientEmail = _appRepository.GetClientEmail<GoogleAnalytics>();

            var credentials = await new OAuth2JsonWebToken<GoogleAnalytics>(privateKey, clientEmail)
                .AddScope<GoogleAnalyticsReports>()
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            var body = new GoogleAnalyticsReportBody
            {
                ReportRequests = new List<GoogleAnalyticsReportRequest>
                {
                    new GoogleAnalyticsReportRequest
                    {
                        DateRanges = new List<GoogleAnalyticsDateRange>
                        {
                            new GoogleAnalyticsDateRange
                            {
                                StartDate = new DateTime(2016, 9, 30),
                                EndDate = new DateTime(2166, 10, 4)
                            }
                        },
                        ViewId = "ga:80938602",
                        Metrics = new List<GoogleAnalyticsMetric>
                        {
                            new GoogleAnalyticsMetric
                            {
                                Expression = "ga:pageviews"
                            }
                        },
                        Dimensions = new List<GoogleAnalyticsDimension>
                        {
                            new GoogleAnalyticsDimension
                            {
                                Name = "ga:date"
                            }
                        }
                    }
                }
            };

            var request = new GoogleAnalyticsReports
            {
                Body = body
            };

            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<GoogleAnalyticsReports, GoogleAnalyticsReportsResponse>(request)
                .ConfigureAwait(false);
            
            Assert.NotNull(response.Reports[0].Data.Rows[0].Metrics[0].Values[0]);
        }

        [Fact]
        public async void MakeRequestForGmailMetadata()
        {
            var credentials = _tokenRepository.GetToken<Google, OAuth2Credentials>();

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
        public async void MakeRequestForGoogleProfile()
        {
            var credentials = _tokenRepository.GetToken<Google, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<GoogleProfile, GoogleProfileResponse>()
                .ConfigureAwait(false);

            Assert.NotNull(response);
            Assert.NotEmpty(response.Emails.First().Value);
        }

        [Fact]
        public async void MakeRequestForGmails()
        {
            var credentials = _tokenRepository.GetToken<Google, OAuth2Credentials>();

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

        #endregion Google Requests

        #region Rescuetime Requests

        [Fact]
        public async void MakeRequestForRescuetimeAnalyticData()
        {
            var credentials = _tokenRepository.GetToken<Rescuetime, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Rescuetime, OAuth2Credentials>();

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

        #endregion Rescuetime Requests

        [Fact]
        public async void MakeRequestForRunkeeperFitnessActivities()
        {
            var credentials = _tokenRepository.GetToken<Runkeeper, OAuth2Credentials>();

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

        #region 23AndMe Requests

        [Fact]
        public async void MakeRequestForTwentyThreeAndMeUser()
        {
            var credentials = _tokenRepository.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

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

        #endregion 23AndMe Requests

        [Fact]
        public async void MakeRequestForSpotifyPlayList()
        {
            var credentials = _tokenRepository.GetToken<Spotify, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new SpotifySavedTrack();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<SpotifySavedTrack, SpotifySavedTrackResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        #region Foursquare Requests

        [Fact]
        public async void MakeRequestForFoursquareCheckins()
        {
            var credentials = _tokenRepository.GetToken<Foursquare, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Foursquare, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Foursquare, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FoursquareTip();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FoursquareTip, FoursquareTipResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        #endregion Foursquare Requests

        #region Fitbit Requests

        [Fact]
        public async void MakeRequestForFitbitIntradaySteps()
        {
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Fitbit, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FitbitProfile();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FitbitProfile, FitbitProfileResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Fitbit Requests

        #region Facebook

        [Fact]
        public async void MakeRequestForFacebookUser()
        {
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<FacebookUser, FacebookUserResponse>()
                .ConfigureAwait(false);

            Assert.NotNull(response);
            Assert.NotEmpty(response.Email);
        }

        [Fact]
        public async void MakeRequestForFacebookLike()
        {
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

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
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

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

        #endregion Facebook

        #region Twitter Requests

        [Fact]
        public async void MakeRequestForTwitterVerifyCredentials()
        {
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterVerifyCredentials();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterVerifyCredentials, TwitterVerifyCredentialsResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
            Assert.NotNull(response.Email);
        }

        [Fact]
        public async void MakeRequestForTwitterTweets()
        {
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

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
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterRetweetOfMe();
            var response = await new OAuthRequester(credentials)
                .MakeOAuthRequestAsync<TwitterRetweetOfMe, TwitterRetweetOfMeResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Twitter Requests
    }
}
