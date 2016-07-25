using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Foundations.Extensions;
using Foundations.Serialization;
using Material.Contracts;
using Material.Infrastructure.Requests;
using GoogleGmailMetadata = Material.Infrastructure.Requests.GoogleGmailMetadata;

namespace Aggregator.Infrastructure.Clients
{
    public class GoogleGmailRecursiveOAuthClient : OAuthClient<GoogleGmail>
    {
        private readonly OAuthClient<GoogleGmailMetadata> _metadataRequest;
         
        public GoogleGmailRecursiveOAuthClient(IOAuthProtectedResource oauthRequest) :
            base(oauthRequest)
        {
            _metadataRequest = new OAuthClient<GoogleGmailMetadata>(oauthRequest);
        }

        public override async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            var metadataRequest = new GoogleGmailMetadata();

            var messageMetadata = 
                await _metadataRequest
                .MakeAuthenticatedRequest(
                    metadataRequest, 
                    recencyValue)
                .ConfigureAwait(false);
            var messages = messageMetadata.SelectToken(
                metadataRequest.PayloadProperty);

            var recursiveRequests = new List<Task<JToken>>();

            var request = new GoogleGmail();
            foreach (var message in messages)
            {
                var recursiveRequest = new GoogleGmail();
                recursiveRequest.PathParameters["messageId"] = 
                        message["id"].ToString();

                recursiveRequests.Add(
                    base.MakeAuthenticatedRequest(
                        recursiveRequest, 
                        string.Empty));
            }

            return recursiveRequests.Select(messageTask =>
            {
                var messageResult = (JObject)messageTask.Result;

                var bodyText = FindBodyText(messageResult);

                var date = messageResult.ExtractTimestamp(
                    request.ResponseTimestamp.TimestampProperty,
                    request.ResponseTimestamp.TimestampFormat,
                    request.ResponseTimestamp.TimestampOffsetProperty,
                    request.ResponseTimestamp.TimestampOffset);

                if (bodyText != null)
                {
                    //TODO: magic strings
                    messageResult["decodedBody"] = 
                        bodyText.FromModifiedBase64String();

                    //For some reason Google assigns a different attachmentId every time a request is made
                    //in order to distinguish differing samples delete the node that specifies this
                    var variableProperties = messageResult
                        .Descendants()
                        .Where(t => t.Type == JTokenType.Property && ((JProperty) t).Name == "attachmentId")
                        .ToArray();
                    foreach (JToken property in variableProperties)
                    {
                        property.Remove();
                    }

                    return new Tuple<DateTimeOffset, JObject>(date, messageResult);
                }
                else
                {
                    return null;
                }
            })
            .Where(d => d != null)
            .ToList();
        }

        private static string FindBodyText(JObject email)
        {
            var bodies = email.AllMatchingProperties("body");
            var bodiesWithData = bodies
                    .Where(a => a["data"] != null);

            var withData = bodiesWithData as JToken[] ?? bodiesWithData.ToArray();
            if (withData.Count() > 1)
            {
                withData = withData
                           .Where(a => a.Parent?.Parent["mimeType"]?.ToString() == "text/plain")
                           .ToArray();
            }

            if (bodies != null &&
                withData.Count() != 0)
            {
                return withData.First()["data"].ToString();
            }
            else
            {
                return null;
            }
        }


    }
}
