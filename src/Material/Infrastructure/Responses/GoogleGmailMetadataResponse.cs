using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class Message
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "threadId")]
        public string ThreadId { get; set; }
    }

    public class GoogleGmailMetadataResponse
    {
        [JsonProperty(PropertyName = "messages")]
        public IList<Message> Messages { get; set; }
        [JsonProperty(PropertyName = "nextPageToken")]
        public string NextPageToken { get; set; }
        [JsonProperty(PropertyName = "resultSizeEstimate")]
        public int ResultSizeEstimate { get; set; }
    }
}
