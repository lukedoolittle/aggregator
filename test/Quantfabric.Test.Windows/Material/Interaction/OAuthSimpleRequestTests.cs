using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Material.Contracts;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.RequestBodies;
using Material.Infrastructure.Requests;
using Material.Infrastructure.Responses;
using Material.OAuth.Workflow;
using Quantfabric.Test.Helpers;
using Quantfabric.Test.Material.Mocks;
using Quantfabric.Test.TestHelpers;
using Xunit;

namespace Quantfabric.Test.Material.Interaction
{
    [Trait("Category", "RequiresToken")]
    public class OAuthSimpleRequestTests
    {
        private readonly TokenCredentialRepository _tokenRepository
            = new TokenCredentialRepository(true);
        private readonly AppCredentialRepository _appRepository
            = new AppCredentialRepository(CallbackType.Localhost);

        #region AppFigures

        [Fact]
        public async void MakeRequestForAppFiguresSales()
        {
            var credentials = _tokenRepository.GetToken<AppFigures, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new AppFiguresSales
            {
                StartDate = new DateTime(2017, 02, 01),
                EndDate = new DateTime(2017, 02, 28),
                Granularity = AppFiguresSalesGranularity.Daily,
                GroupBy = AppFiguresSalesGroupBy.Product
            };
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<AppFiguresSales, AppFiguresSalesResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion AppFigures

        #region Yahoo

        [Fact]
        public async void MakeRequestForYahooFlurryMetrics()
        {
            var credentials = _appRepository.GetApiKeyCredentials<YahooFlurry>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new YahooFlurryMetrics
            {
                Table = YahooFlurryMetricsTable.AppUsage,
                TimeGrain = YahooFlurryMetricsTimeGrain.Day
            };
            request.AddDimension(YahooFlurryMetricsDimension.App)
                .AddDimension(YahooFlurryMetricsDimension.Region)
                .AddMetric(YahooFlurryMetricsMetrics.Sessions)
                .AddMetric(YahooFlurryMetricsMetrics.TimeSpent)
                .AddDateRange(new DateTime(2017, 02, 01), new DateTime(2017, 02, 28));
            
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<YahooFlurryMetrics, YahooFlurryMetricsResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Yahoo

        #region Microsoft

        [Fact]
        public async void MakeRequestForMicrosoftTable()
        {
            var accountName = "musicnotes";
            var tableName = "TestTable";

            var credentials = new AccountKeyCredentials()
                .AddAccountInformation(
                    _appRepository.GetAccountName<AzureTableStorage>(),
                    _appRepository.GetAccountKey<AzureTableStorage>(), 
                    new AzureTableStorage().KeyName);

            var request = new AzureTableStorageQuery
            {
                TableName = tableName
            }.SetAccount(accountName);

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<AzureTableStorageQuery, AzureTableStorageQueryResponse<SampleTableStorageEntity>>(request)
                .ConfigureAwait(false);

            var entities = response.Value;

            var entity = entities.First();

            Assert.Equal("WillieDoolittle", entity.Name);
            Assert.Equal(new DateTime(2016, 10, 03), entity.Timestamp);
            Assert.Equal("1", entity.PartitionKey);
            Assert.Equal("1", entity.RowKey);
        }

        [Fact]
        public async void MakeRequestForMicrosoftTablePost()
        {
            var accountName = "musicnotes";
            var tableName = "TestTable";
            var timestamp = new DateTime(2016, 10, 03);

            var credentials = new AccountKeyCredentials()
                .AddAccountInformation(
                    _appRepository.GetAccountName<AzureTableStorage>(),
                    _appRepository.GetAccountKey<AzureTableStorage>(),
                    new AzureTableStorage().KeyName);

            var entity = new SampleTableStorageEntity("1", "1")
            {
                Name = "WillieDoolittle",
                Timestamp = timestamp
            };

            var request = new AzureTableStorageNonQuery
            {
                TableName = tableName
            }.SetAccount(accountName).SetBody(entity);

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync(request)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(Skip="This isn't implemented")]
        public async void MakeRequestForMicrosoftTableMultiplePost()
        {
            var accountName = "musicnotes";
            //var tableName = "TestTable";
            var timestamp = new DateTime(2016, 10, 03);

            var credentials = new AccountKeyCredentials()
                .AddAccountInformation(
                    _appRepository.GetAccountName<AzureTableStorage>(),
                    _appRepository.GetAccountKey<AzureTableStorage>(),
                    new AzureTableStorage().KeyName);

            var entity1 = new SampleTableStorageEntity("1", "1")
            {
                Name = "WillieDoolittle",
                Timestamp = timestamp
            };

            var entity2 = new SampleTableStorageEntity("1", "2")
            {
                Name = "BillDoolittle",
                Timestamp = timestamp
            };

            var request = new AzureTableStorageBatch()
            {
            }.SetAccount(accountName);

            request.SetBody(new List<SampleTableStorageEntity> {entity1, entity2});

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync(request)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }


        #endregion Microsoft

        #region Xamarin Insights

        [Fact]
        public async void MakeRequestForXamarinInsightsUsersThenSessionsThenEvents()
        {
            var username = _appRepository.GetUsername<XamarinInsights>();
            var password = _appRepository.GetPassword<XamarinInsights>();
            var appId = "0772a1b8bdf2430d6f7faa4cbb7bd6e1baa13831";

            var credentials = await new SimplePassword<XamarinInsights>(
                    username,
                    password)
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            var userRequest = new XamarinInsightsUsers
            {
                AppId = appId
            };

            var userResponse = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<XamarinInsightsUsers, XamarinUserResponse>(userRequest)
                .ConfigureAwait(false);

            var user = userResponse.Hits.HitList.First().Source;
            var userId = user.Id;
            var someDeviceId = user.Devices.First().Id;

            var sessionRequest = new XamarinInsightsSessions
            {
                AppId = appId,
                Deviceid = someDeviceId.ToString(),
                Userid = userId
            };

            var sessionResponse = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<XamarinInsightsSessions, XamarinSessionResponse>(sessionRequest)
                .ConfigureAwait(false);

            var session = sessionResponse.Hits.Hits.First();

            var eventsRequest = new XamarinInsightsEvents()
            {
                AppId = appId,
                Deviceid = someDeviceId.ToString(),
                Userid = userId,
                Sessionid = session.Id
            };

            var eventsResponse = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<XamarinInsightsEvents, XamarinEventResponse>(eventsRequest)
                .ConfigureAwait(false);

            Assert.NotNull(eventsResponse.Hits.Hits.First());
        }

        #endregion Xamarin Insights

        #region Linkedin Requests

        [Fact]
        public async void MakeRequestForLinkedInPersonal()
        {
            var credentials = _tokenRepository.GetToken<LinkedIn, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new LinkedinPersonal();
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<FatsecretMeal, FatsecretMealResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Fatsecret Requests

        #region Omniture

        [Fact]
        public async void MakeRequestForOmnitureAggregateReports()
        {
            var credentials = _tokenRepository.GetToken<Omniture, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var body = new OmnitureQueueBody
            {
                ReportDescription = new OmnitureReportDescription
                {
                    DateGranularity = OmnitureReportingDateGranularity.Hour,
                    ReportSuiteId = "musicnotes",
                    StartDate = new DateTime(2016, 9, 6),
                    EndDate = new DateTime(2016, 10, 6),
                    Metrics = new List<OmnitureMetric> { new OmnitureMetric { Id = "visits" } }
                }
            };

            var request = new OmnitureReporting
            {
                Method = OmnitureReportingMethod.ReportQueue
            };
            request.AddContent(body);

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<OmnitureReporting, OmnitureQueueResponse>(request)
                .ConfigureAwait(false);

            System.Threading.Thread.Sleep(10000);

            var getBody = new OmnitureGetBody
            {
                ReportId = response.ReportId
            };

            request = new OmnitureReporting
            {
                Method = OmnitureReportingMethod.ReportGet
            };
            request.AddContent(getBody);

            var getResponse = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<OmnitureReporting, OmnitureGetResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(getResponse.Report.Totals[0]);
        }

        #endregion Omniture

        #region Amazon

        [Fact]
        public async void MakeRequestForAmazonProfile()
        {
            var credentials = _tokenRepository.GetToken<Amazon, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<AmazonProfile, AmazonProfileResponse>()
                .ConfigureAwait(false);

            Assert.NotNull(response.UserId);
        }

        #endregion Amazon

        #region Tumblr

        [Fact]
        public async void MakeRequestForTumblrLikes()
        {
            var credentials = _tokenRepository.GetToken<Tumblr, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<TumblrLikes, TumblrLikeResponse>()
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Tumblr

        #region Withings

        [Fact]
        public async void MakeRequestForWithingsWeighins()
        {
            var credentials = _tokenRepository.GetToken<Withings, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new WithingsWeighIn()
            {
                Lastupdate = DateTime.Today.Subtract(TimeSpan.FromDays(200))
            };

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<WithingsWeighIn, WithingsWeighInResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Withings

        #region Google Requests

        [Fact]
        public async void MakeRequestForGoogleAnalyticsReports()
        {
            var privateKey = _appRepository.GetPrivateKey<GoogleAnalytics>();
            var clientEmail = _appRepository.GetClientEmail<GoogleAnalytics>();

            var credentials = await new OAuth2Assert<GoogleAnalytics>(
                    new RsaCryptoKey(
                        privateKey, 
                        true, 
                        StringEncoding.Base64),
                    clientEmail)
                .AddScope<GoogleAnalyticsReports>()
                .GetCredentialsAsync(JsonWebTokenAlgorithm.RS256)
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

            var request = new GoogleAnalyticsReports();
            request.AddContent(body);

            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<GoogleGmailMetadata, GoogleGmailMetadataResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact]
        public async void MakeRequestForGoogleProfile()
        {
            var credentials = _tokenRepository.GetToken<Google, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var response = await new AuthorizedRequester(credentials)
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

            var metadataResponse = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<GoogleGmailMetadata, GoogleGmailMetadataResponse>()
                .ConfigureAwait(false);

            foreach (var message in metadataResponse.Messages.Take(5))
            {
                var request = new GoogleGmail {MessageId = message.Id};
                var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<RescuetimeAnalyticData, RescuetimeAnalyticDataResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        #endregion Rescuetime Requests

        #region Runkeeper

        [Fact]
        public async void MakeRequestForRunkeeperFitnessActivities()
        {
            var credentials = _tokenRepository.GetToken<Runkeeper, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new RunkeeperFitnessActivity()
            {
                NoEarlierThan = DateTime.Today.Subtract(TimeSpan.FromDays(60))
            };
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<RunkeeperFitnessActivity, RunkeeperFitnessActivityResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        #endregion Runkeeper

        #region 23AndMe Requests

        [Fact]
        public async void MakeRequestForTwentyThreeAndMeUser()
        {
            var credentials = _tokenRepository.GetToken<TwentyThreeAndMe, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwentyThreeAndMeUser();
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<TwentyThreeAndMeGenome, TwentyThreeAndMeGenomeResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        #endregion 23AndMe Requests

        #region Spotify

        [Fact]
        public async void MakeRequestForSpotifyPlayList()
        {
            var credentials = _tokenRepository.GetToken<Spotify, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new SpotifySavedTrack();
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<SpotifySavedTrack, SpotifySavedTrackResponse>(request)
                .ConfigureAwait(false);
            Assert.NotNull(response);
        }

        #endregion Spotify

        #region Foursquare Requests

        [Fact]
        public async void MakeRequestForFoursquareCheckins()
        {
            var credentials = _tokenRepository.GetToken<Foursquare, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FoursquareCheckin();
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<FitbitProfile, FitbitProfileResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Fitbit Requests

        #region Facebook

        [Fact]
        public async void MakeRequestForFacebookTokenInfo()
        {
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new FacebookTokenInfo
            {
                InputToken = credentials.AccessToken,
            };

            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<FacebookTokenInfo, FacebookTokenInfoResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
            Assert.NotNull(response.Data.UserId);
        }

        [Fact]
        public async void MakeRequestForFacebookUser()
        {
            var credentials = _tokenRepository.GetToken<Facebook, OAuth2Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<TwitterVerifyCredentials, TwitterVerifyCredentialsResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        [Fact(Skip="Requires special twitter permissions")]
        public async void MakeRequestForTwitterVerifyCredentialsWithEmail()
        {
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterVerifyCredentials();
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<TwitterVerifyCredentials, TwitterVerifyCredentialsResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response.Email);
        }

        [Fact]
        public async void MakeRequestForTwitterTweets()
        {
            var credentials = _tokenRepository.GetToken<Twitter, OAuth1Credentials>();

            if (credentials.IsTokenExpired) { throw new Exception("Expired credentials!!!"); }

            var request = new TwitterTweet();
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
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
            var response = await new AuthorizedRequester(credentials)
                .MakeOAuthRequestAsync<TwitterRetweetOfMe, TwitterRetweetOfMeResponse>(request)
                .ConfigureAwait(false);

            Assert.NotNull(response);
        }

        #endregion Twitter Requests
    }
}
