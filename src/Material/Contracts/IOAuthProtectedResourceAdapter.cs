using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Foundations.Enums;

namespace Material.Contracts
{
    public interface IOAuthProtectedResourceAdapter
    {
        Task<TEntity> ForProtectedResource<TEntity>(
            string host,
            string path,
            string httpMethod,
            IEnumerable<MediaType> responseTypes,
            IDictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> urlPathParameters,
            object body,
            MediaType bodyType,
            IEnumerable<HttpStatusCode> expectedResponse,
            MediaType? overriddenMediaType);
    }
}
