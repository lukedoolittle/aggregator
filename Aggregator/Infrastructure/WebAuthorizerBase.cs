using System;
using System.Linq;
using System.Threading.Tasks;
using BatmansBelt.Serialization;
using Aggregator.Framework.Enums;
using Aggregator.Framework.Serialization;
using Aggregator.Infrastructure.Credentials;
using RestSharp.Extensions.MonoHttp;

namespace Aggregator.Infrastructure
{
    public abstract class WebAuthorizerBase
    {
        protected const string _responseText = "You give me Aggregator fever...chomp...chomp";
        protected const MimeTypeEnum _responseType = MimeTypeEnum.Text;

        protected void HandleResponse<TToken>(
            TaskCompletionSource<TToken> taskCompletion, 
            Uri responseUri)
            where TToken : TokenCredentials
        {
            if (!string.IsNullOrEmpty(responseUri.Fragment))
            {
                var token = HttpUtility.ParseQueryString(
                    responseUri.Fragment)
                    .AsEntity<TToken>();

                taskCompletion.SetResult(token);
            }

            else if (!string.IsNullOrEmpty(responseUri.Query))
            {
                var querystringCollection = HttpUtility.ParseQueryString(
                    responseUri.Query);
                var token = querystringCollection.AsEntity<TToken>();

                //TODO: this may be overly aggressive and there may be a better way to do this directly with serialization in newtonsoft
                var labeledProperties = token.AsJObject().Properties().Select(p => p.Name);
                var unmappedQuerystringParameters = querystringCollection.AllKeys.Where(k => !labeledProperties.Contains(k));
                foreach (var unmappedParameter in unmappedQuerystringParameters)
                {
                    token.AdditionalTokenParameters.Add(
                        unmappedParameter, 
                        querystringCollection[unmappedParameter]);
                }

                taskCompletion.SetResult(token);
            }
        }
    }
}
