using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;

namespace Aggregator.Infrastructure.Clients
{
    public class OAuthClient<TRequest> : 
        IRequestClient
        where TRequest : OAuthRequest, new()
    {
        protected readonly IOAuth _oauthRequester;

        public OAuthClient(IOAuth oauthRequester)
        {
            _oauthRequester = oauthRequester;
        }

        public virtual async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            return await GetDataPoints(recencyValue, new TRequest());
        }

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue,
            TRequest request)
        {
            var response = await MakeAuthenticatedRequest(
                            request,
                            recencyValue)
                            .ConfigureAwait(false);

            var payload = response.SelectToken(request.PayloadProperty);

            return ExtractDataPoints(payload, request.ResponseTimestamp);
        }

        protected virtual IEnumerable<Tuple<DateTimeOffset, JObject>> ExtractDataPoints(
            JToken payload, 
            TimestampOptions options)
        {
            var dataPoints = payload
                .InContainer()
                .Select(p =>
                    new Tuple<DateTimeOffset, JObject>(
                        p.ExtractTimestamp(
                            options.TimestampProperty,
                            options.TimestampFormat,
                            options.TimestampOffsetProperty,
                            options.TimestampOffset),
                        p));

            return dataPoints;
        } 

        public virtual Task<JToken> MakeAuthenticatedRequest(
            TRequest request,
            string recencyValue)
        {
            return _oauthRequester.ForProtectedResource(
                request.Host,
                request.Path,
                request.HttpMethod,
                request.RequestFilterKey,
                request.Headers,
                request.QuerystringParameters,
                request.AdditionalUrlSegmentParameters,
                recencyValue);
        }
    }
}
