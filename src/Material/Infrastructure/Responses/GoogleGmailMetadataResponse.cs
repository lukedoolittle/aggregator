using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "threadId")]
        [JsonProperty(PropertyName = "threadId")]
        public string ThreadId { get; set; }
    }

    [DataContract]
    public class GoogleGmailMetadataResponse
    {
        [DataMember(Name = "messages")]
        [JsonProperty(PropertyName = "messages")]
        public IList<Message> Messages { get; set; }
        [DataMember(Name = "nextPageToken")]
        [JsonProperty(PropertyName = "nextPageToken")]
        public string NextPageToken { get; set; }
        [DataMember(Name = "resultSizeEstimate")]
        [JsonProperty(PropertyName = "resultSizeEstimate")]
        public int ResultSizeEstimate { get; set; }
    }
}
