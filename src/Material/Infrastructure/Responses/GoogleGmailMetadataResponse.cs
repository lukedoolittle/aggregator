using System.Runtime.Serialization;
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
        public string Id { get; set; }
        [DataMember(Name = "threadId")]
        public string ThreadId { get; set; }
    }

    [DataContract]
    public class GoogleGmailMetadataResponse
    {
        [DataMember(Name = "messages")]
        public IList<Message> Messages { get; set; }
        [DataMember(Name = "nextPageToken")]
        public string NextPageToken { get; set; }
        [DataMember(Name = "resultSizeEstimate")]
        public int ResultSizeEstimate { get; set; }
    }
}
