using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Http;

namespace Material.Contracts
{
    public interface IOAuthProtectedResource
    {
        Task<TEntity> ForProtectedResource<TEntity>(
            string baseUrl,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> additionalUrlSegmentParameters,
            object body,
            MediaTypeEnum bodyType,
            HttpStatusCode expectedResponse = HttpStatusCode.OK,
            MediaTypeEnum expectedResponseType = MediaTypeEnum.Json);
    }
}
