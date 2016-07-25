using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Foundations.Extensions;
using Material.Contracts;
using Material.Infrastructure;

namespace Aggregator.Infrastructure.Clients
{
    public abstract class FitbitIntradayOAuthClient<TFitbitRequest> :
        FitbitOAuthClient<TFitbitRequest>
        where TFitbitRequest : OAuthRequest, new()
    {
        //TODO: magic strings and magic numbers
        protected FitbitIntradayOAuthClient(IOAuthProtectedResource oauthRequest) :
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
                //TODO: have to switch request types here to bulk request
                var pastDays = TimeSpan.FromDays(daysInPast);
                request.PathParameters["date"] =
                    DateTime.Now.Subtract(pastDays).ToString("yyyy-MM-dd");
            }
            else
            {
                var startTime = DateTimeOffset.Parse(recencyValue);

                request.PathParameters["startdate"] =
                    startTime.ToString("yyyy-MM-dd");
                request.PathParameters["enddate"] =
                    DateTime.Now.ToString("yyyy-MM-dd");
                request.PathParameters["starttime"] =
                    startTime.ToString("HH:mm");
                request.PathParameters["endtime"] =
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

            var date = request.PathParameters.ContainsKey("date") ? 
                DateTime.Parse(request.PathParameters["date"]) : 
                DateTime.Parse(request.PathParameters["startdate"]);
                       
            var timestampFormat = request.ResponseTimestamp.TimestampFormat;
            var results = new List<Tuple<DateTimeOffset, JObject>>();
            foreach (var dataPoint in payload.InContainer())
            {
                if (dataPoint["time"].ToString() == "00:00" &&
                    request.PathParameters.ContainsKey("enddate"))
                {
                    date = DateTime.Parse(request.PathParameters["enddate"]);
                }

                var offsetString = offset.ToString("hhmm");
                if (!offsetString.StartsWith("-"))
                {
                    offsetString = "+" + offsetString;
                }
                var datetime = $"{date.ToString("yyyy-MM-dd")} {dataPoint["time"]} {offsetString}";

                var timestamp = datetime.ToDateTimeOffset(timestampFormat, null);
                dataPoint["dateTime"] = datetime;
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

        protected FitbitOAuthClient(IOAuthProtectedResource oauthRequester) :
            base(oauthRequester)
        {
        }

        public override async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            var offset = DateTimeOffset.Now.Offset;

            if (string.IsNullOrEmpty(recencyValue))
            {
                return await GetDataPointsDayByDay(_backdatedDays, offset)
                    .ConfigureAwait(false);
            }
            else
            {
                var pastDate = DateTimeOffset.Parse(recencyValue);
                var now = DateTimeOffset.Now;

                var daysSinceLastRequest = Convert.ToInt32(Math.Floor((now - pastDate).TotalDays));

                if (daysSinceLastRequest > 0)
                {
                    return await GetDataPointsDayByDay(daysSinceLastRequest, offset)
                        .ConfigureAwait(false);
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

