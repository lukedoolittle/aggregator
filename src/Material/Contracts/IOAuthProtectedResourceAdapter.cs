using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations;
using Foundations.Enums;

namespace Material.Contracts
{
    public interface IOAuthProtectedResourceAdapter
    {
        Task<TEntity> ForProtectedResource<TEntity>(
            string baseUrl,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> additionalUrlSegmentParameters,
            object body,
            MediaType bodyType,
            HttpStatusCode expectedResponse = HttpStatusCode.OK,
            MediaType expectedResponseType = MediaType.Json);
    }
}
