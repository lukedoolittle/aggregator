using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Material.Contracts;
using Material.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class FitbitSleepOAuthClient : FitbitOAuthClient<FitbitSleep>
    {
        private const string DateConstant = "date";
        private const string DateTime = "dateTime";
        private const string DateFormat = "yyyy-MM-dd";
        private const int BackdatedDays = 7;

        public FitbitSleepOAuthClient(IOAuthProtectedResource oauthRequest) :
            base(oauthRequest)
        {
            _backdatedDays = BackdatedDays;
        }

        protected override FitbitSleep CreateRequest(
            string recencyValue,
            int daysInPast = 0)
        {
            var request = new FitbitSleep();
            string date;

            if (string.IsNullOrEmpty(recencyValue))
            {
                var pastDays = TimeSpan.FromDays(daysInPast);
                date = System.DateTime.Today.Subtract(pastDays)
                    .ToString(DateFormat);
            }
            else
            {
                var startTime = Convert.ToDateTime(recencyValue);
                date = startTime.ToString(DateFormat);
            }
            request.PathParameters[DateConstant] = date;
            return request;
        }

        protected override IEnumerable<Tuple<DateTimeOffset, JObject>> FormatResult(
            FitbitSleep request,
            JToken result, 
            TimeSpan offset)
        {
            var payload = result.SelectToken(request.PayloadProperty);

            if (payload == null)
            {
                return new List<Tuple<DateTimeOffset, JObject>>();
            }

            var startingTime = Convert.ToDateTime(
                request.PathParameters[DateConstant]);

            var results = new List<Tuple<DateTimeOffset, JObject>>();
            foreach (var dataPoint in payload.InContainer())
            {
                var time = Convert.ToDateTime(dataPoint[DateTime]);

                var timestamp = new DateTimeOffset(
                    new DateTime(
                        startingTime.Year,
                        startingTime.Month,
                        startingTime.Day,
                        time.Hour,
                        time.Minute,
                        time.Second,
                        DateTimeKind.Unspecified),
                    offset);
                dataPoint[DateTime] = timestamp.ToString(request.ResponseTimestamp.TimestampFormat);
                results.Add(new Tuple<DateTimeOffset, JObject>(timestamp, dataPoint));
            }

            return results;
        }
    }
}

