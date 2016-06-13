using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class RescuetimeOAuthClient : OAuthClient<RescuetimeAnalyticData>
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";
        private const int BACKDATED_DAYS = 7;
        private const string ROW_HEADERS = "row_headers";
        private const string ROWS = "rows";

        public RescuetimeOAuthClient(IOAuth oauthRequest) :
            base(oauthRequest)
        {
        }

        public override async Task<JToken> MakeAuthenticatedRequest(
            RescuetimeAnalyticData request,
            string recencyValue)
        {
            var currentRequest = CreateRequest(request, recencyValue);

            var rawResult = await base.MakeAuthenticatedRequest(
                    currentRequest,
                    recencyValue)
                .ConfigureAwait(false);

            return FormatResult(rawResult);
        }

        private RescuetimeAnalyticData CreateRequest(
            RescuetimeAnalyticData currentRequest,
            string recencyValue)
        {
            if (string.IsNullOrEmpty(recencyValue))
            {
                currentRequest.QuerystringParameters[currentRequest.RequestFilterKey] =
                    DateTime.Now.Subtract(
                        TimeSpan.FromDays(BACKDATED_DAYS)).ToString(DATE_FORMAT);
                currentRequest.QuerystringParameters["restrict_end"] =
                        DateTime.Now.ToString(DATE_FORMAT);
            }
            else
            {
                currentRequest.QuerystringParameters[currentRequest.RequestFilterKey] =
                    DateTime.Parse(recencyValue).ToString(DATE_FORMAT);
            }

            return currentRequest;
        }

        private JToken FormatResult(JToken rawResult)
        {
            var headers = (JArray)rawResult[ROW_HEADERS];
            var data = (JArray)rawResult[ROWS];

            var result = new JArray();

            if (headers == null ||
                data == null)
            {
                return result;
            }

            foreach (var jToken in data)
            {
                var datum = (JArray)jToken;
                var row = new JObject();
                result.Add(row);

                for (var i = 0; i < headers.Count; i++)
                {
                    var key = (headers[i] as JValue)?.Value.ToString();
                    var value = (datum[i] as JValue)?.Value.ToString();
                    row[key] = value;
                }
            }

            return result;
        }
    }
}
