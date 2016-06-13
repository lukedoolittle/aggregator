using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class FitbitSleepOAuthClient : FitbitOAuthClient<FitbitSleep>
    {
        public FitbitSleepOAuthClient(IOAuth oauthRequest) :
            base(oauthRequest)
        {
            _backdatedDays = FitbitSleep.BackdatedDays;
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
                date = DateTime.Today.Subtract(pastDays)
                    .ToString(FitbitSleep.DateFormat);
            }
            else
            {
                var startTime = Convert.ToDateTime(recencyValue);
                date = startTime.ToString(FitbitSleep.DateFormat);
            }
            request.AdditionalUrlSegmentParameters[FitbitSleep.Date] = date;
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
                request.AdditionalUrlSegmentParameters[FitbitSleep.Date]);

            return
                from dataPoint in payload.InContainer()
                let time = Convert.ToDateTime(dataPoint[FitbitSleep.DateTime])
                let timestamp = new DateTimeOffset(
                    new DateTime(
                        startingTime.Year,
                        startingTime.Month,
                        startingTime.Day,
                        time.Hour,
                        time.Minute,
                        time.Second,
                        DateTimeKind.Unspecified),
                    offset)
                select new Tuple<DateTimeOffset, JObject>(timestamp, dataPoint);
        }
    }
}
