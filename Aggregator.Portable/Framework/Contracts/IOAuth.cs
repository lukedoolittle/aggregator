using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Aggregator.Framework.Contracts
{
    public interface IOAuth
    {
        Task<JToken> ForProtectedResource(
            string baseUrl,
            string path,
            string httpMethod,
            string filter,
            Dictionary<string, string> headers,
            Dictionary<string, string> additionalQuerystringParameters,
            Dictionary<string, string> additionalUrlSegmentParameters,
            string recencyValue = "");
    }
}
