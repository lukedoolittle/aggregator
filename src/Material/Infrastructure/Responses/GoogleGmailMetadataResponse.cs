using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Message
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "threadId")]
        public string ThreadId { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
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
