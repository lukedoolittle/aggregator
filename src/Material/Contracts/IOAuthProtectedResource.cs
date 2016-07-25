using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Material.Contracts
{
    public interface IOAuthProtectedResource
    {
        Task<string> ForProtectedResource(
            string baseUrl,
            string path,
            string httpMethod,
            Dictionary<HttpRequestHeader, string> headers,
            IDictionary<string, string> additionalQuerystringParameters,
            IDictionary<string, string> additionalUrlSegmentParameters,
            HttpStatusCode expectedResponse = HttpStatusCode.OK);
    }
}
