using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class TwitterIdOAuthClient : OAuthClient<TwitterTweet>
    {
        private readonly OAuthClient<TwitterId> _idClient;

        public TwitterIdOAuthClient(
            IOAuth oauthRequest, 
            OAuthClient<TwitterId> idClient) :
            base(oauthRequest)
        {
            _idClient = idClient;
        }

        public override async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            var request = new TwitterTweet();

            //also when authenticating with twitter, this user id is passed back
            //so maybe we can get it as part of the credentials???
            if (!request.QuerystringParameters.ContainsKey(TwitterTweet.IdParameter) || 
                request.QuerystringParameters[TwitterTweet.IdParameter] == string.Empty)
            {
                var idRequest = new TwitterId();
                var idResult = await _idClient
                    .MakeAuthenticatedRequest(idRequest, null)
                    .ConfigureAwait(false);

                request.QuerystringParameters[TwitterTweet.IdParameter] = idResult
                    .SelectToken(idRequest.PayloadProperty)
                    .ToString();
            }

            return await base.GetDataPoints(recencyValue)
                .ConfigureAwait(false);
        }
    }
}
