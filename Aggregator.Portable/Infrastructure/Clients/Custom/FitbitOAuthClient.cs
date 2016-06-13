using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;

namespace Aggregator.Infrastructure.Clients
{
    public abstract class FitbitIntradayOAuthClient<TFitbitRequest> :
        FitbitOAuthClient<TFitbitRequest>
        where TFitbitRequest : OAuthRequest, new()
    {
        //TODO: magic strings and magic numbers
        protected FitbitIntradayOAuthClient(IOAuth oauthRequest) :
            base(oauthRequest)
        {
            _backdatedDays = 7;
        }

        protected override TFitbitRequest CreateRequest(
            string recencyValue = "",
            int daysInPast = 0)
        {
            var request = new TFitbitRequest();

            if (string.IsNullOrEmpty(recencyValue))
            {
                request.IsBulkRequest = true;
                var pastDays = TimeSpan.FromDays(daysInPast);
                request.AdditionalUrlSegmentParameters["date"] =
                    DateTime.Now.Subtract(pastDays).ToString("yyyy-MM-dd");
            }
            else
            {
                var startTime = DateTimeOffset.Parse(recencyValue);

                request.AdditionalUrlSegmentParameters["startdate"] =
                    startTime.ToString("yyyy-MM-dd");
                request.AdditionalUrlSegmentParameters["enddate"] =
                    DateTime.Now.ToString("yyyy-MM-dd");
                request.AdditionalUrlSegmentParameters["starttime"] =
                    startTime.ToString("HH:mm");
                request.AdditionalUrlSegmentParameters["endtime"] =
                    DateTime.Now.ToString("HH:mm");
            }

            return request;
        }

        protected override IEnumerable<Tuple<DateTimeOffset, JObject>> FormatResult(
            TFitbitRequest request,
            JToken result,
            TimeSpan offset)
        {
            var payload = result.SelectToken(request.PayloadProperty);

            var date = request.AdditionalUrlSegmentParameters.ContainsKey("date") ? 
                DateTime.Parse(request.AdditionalUrlSegmentParameters["date"]) : 
                DateTime.Parse(request.AdditionalUrlSegmentParameters["startdate"]);
                       
            var timestampFormat = request.ResponseTimestamp.TimestampFormat;
            var results = new List<Tuple<DateTimeOffset, JObject>>();
            foreach (var dataPoint in payload.InContainer())
            {
                if (dataPoint["time"].ToString() == "00:00" &&
                    request.AdditionalUrlSegmentParameters.ContainsKey("enddate"))
                {
                    date = DateTime.Parse(request.AdditionalUrlSegmentParameters["enddate"]);
                }
                var datetime = $"{date.ToString("yyyy-MM-dd")} {dataPoint["time"]} {offset.ToOffsetString()}";

                var timestamp = datetime.ToDateTimeOffset(timestampFormat, null);
                dataPoint["dateTime"] = timestamp;
                results.Add(new Tuple<DateTimeOffset, JObject>(timestamp, dataPoint));
            }
            return results;
        }
    }

    public abstract class FitbitOAuthClient<TFitbitRequest> : 
        OAuthClient<TFitbitRequest>
        where TFitbitRequest : OAuthRequest, new()
    {
        protected int _backdatedDays { get; set; }

        protected FitbitOAuthClient(IOAuth oauthRequester) :
            base(oauthRequester)
        {
        }

        public override async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            var offset = DateTimeOffset.Now.Offset;

            if (string.IsNullOrEmpty(recencyValue))
            {
                return await GetDataPointsDayByDay(_backdatedDays, offset);
            }
            else
            {
                var daysSinceLastRequest = DateTimeExtensions.DaysSince(recencyValue);
                if (daysSinceLastRequest > 0)
                {
                    return await GetDataPointsDayByDay(daysSinceLastRequest, offset);
                }
                else
                {
                    var request = CreateRequest(recencyValue);

                    var result = await MakeAuthenticatedRequest(
                        request,
                        recencyValue)
                        .ConfigureAwait(false);

                    return FormatResult(
                        request,
                        result,
                        offset);
                }
            }
        }

        private async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPointsDayByDay(
            int daysInPast,
            TimeSpan offset)
        {
            var allItems = new List<Tuple<DateTimeOffset, JObject>>();
            for (var i = daysInPast; i >= 0; i--)
            {
                var request = CreateRequest(daysInPast: i);

                var result = await MakeAuthenticatedRequest(
                    request,
                    string.Empty)
                    .ConfigureAwait(false);

                allItems.AddRange(
                    FormatResult(
                        request,
                        result,
                        offset));
            }

            return allItems;
        }

        protected abstract IEnumerable<Tuple<DateTimeOffset, JObject>> FormatResult(
            TFitbitRequest request,
            JToken result,
            TimeSpan offset);

        protected abstract TFitbitRequest CreateRequest(
            string recencyValue = "",
            int daysInPast = 0);
    }
}
