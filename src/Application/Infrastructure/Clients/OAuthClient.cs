using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Foundations.Serialization;
using Material.Contracts;
using Material.Infrastructure;

namespace Aggregator.Infrastructure.Clients
{
    public class TimeseriesOAuthClient<TRequest> :
        OAuthClient<TRequest>
        where TRequest : OAuthRequest, ITimeseries, new()
    {
        public TimeseriesOAuthClient(IOAuthProtectedResource oauthRequester) : 
            base(oauthRequester)
        {}

        public override async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            TRequest request,
            string recencyValue)
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
            if (options == null)
            {
                return new List<Tuple<DateTimeOffset, JObject>>
                {
                    new Tuple<DateTimeOffset, JObject>(
                        DateTimeOffset.Now,
                        (JObject)payload)
                };
            }

            return payload
                .InContainer()
                .Select(p =>
                    new Tuple<DateTimeOffset, JObject>(
                        p.ExtractTimestamp(
                            options.TimestampProperty,
                            options.TimestampFormat,
                            options.TimestampOffsetProperty,
                            options.TimestampOffset),
                        p));
        }
    }

    public class OAuthClient<TRequest> : 
        IRequestClient
        where TRequest : OAuthRequest, new()
    {
        protected readonly IOAuthProtectedResource _oauthRequester;

        public OAuthClient(IOAuthProtectedResource oauthRequester)
        {
            _oauthRequester = oauthRequester;
        }

        public virtual async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue = null)
        {
            return await GetDataPoints(
                    new TRequest(),
                    recencyValue)
                .ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            TRequest request,
            string recencyValue)
        {
            var response = await MakeAuthenticatedRequest(
                            request)
                            .ConfigureAwait(false);

            var payload = response.SelectToken(request.PayloadProperty);

            return new List<Tuple<DateTimeOffset, JObject>>
            {
                new Tuple<DateTimeOffset, JObject>(
                    DateTimeOffset.Now, 
                    payload.ToObject<JObject>())
            };
        }

        public virtual async Task<JToken> MakeAuthenticatedRequest(
            TRequest request,
            string recencyValue = null)
        {
            //TODO: fix this after implementing filterable strategy
            //the below with the additional querystring parameters will not work
            var requestFilterKey = (request as IFilterable)?.RequestFilterKey;

            if (requestFilterKey != string.Empty &&
                !string.IsNullOrEmpty(recencyValue))
            {
                request.QuerystringParameters.Add(
                    requestFilterKey, 
                    recencyValue);
            }

            var result = await _oauthRequester.ForProtectedResource(
                request.Host,
                request.Path,
                request.HttpMethod,
                request.Headers,
                request.QuerystringParameters,
                request.PathParameters).ConfigureAwait(false);

            return result.AsEntity<JToken>(false);
        }
    }
}
