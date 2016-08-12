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
    public class EmailBody
    {
        [DataMember(Name = "size")]
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        [DataMember(Name = "data")]
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }

    [DataContract]
    public class EmailHeader
    {
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "value")]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class EmailPart
    {
        [DataMember(Name = "partId")]
        [JsonProperty(PropertyName = "partId")]
        public string PartId { get; set; }
        [DataMember(Name = "mimeType")]
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType { get; set; }
        [DataMember(Name = "filename")]
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
        [DataMember(Name = "headers")]
        [JsonProperty(PropertyName = "headers")]
        public IList<EmailHeader> Headers { get; set; }
        [DataMember(Name = "body")]
        [JsonProperty(PropertyName = "body")]
        public EmailBody Body { get; set; }
    }

    [DataContract]
    public class EmailPayload
    {
        [DataMember(Name = "mimeType")]
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType { get; set; }
        [DataMember(Name = "filename")]
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
        [DataMember(Name = "headers")]
        [JsonProperty(PropertyName = "headers")]
        public IList<EmailHeader> Headers { get; set; }
        [DataMember(Name = "body")]
        [JsonProperty(PropertyName = "body")]
        public EmailBody Body { get; set; }
        [DataMember(Name = "parts")]
        [JsonProperty(PropertyName = "parts")]
        public IList<EmailPart> Parts { get; set; }
    }

    [DataContract]
    public class GoogleGmailResponse
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "threadId")]
        [JsonProperty(PropertyName = "threadId")]
        public string ThreadId { get; set; }
        [DataMember(Name = "labelIds")]
        [JsonProperty(PropertyName = "labelIds")]
        public IList<string> LabelIds { get; set; }
        [DataMember(Name = "snippet")]
        [JsonProperty(PropertyName = "snippet")]
        public string Snippet { get; set; }
        [DataMember(Name = "historyId")]
        [JsonProperty(PropertyName = "historyId")]
        public string HistoryId { get; set; }
        [DataMember(Name = "internalDate")]
        [JsonProperty(PropertyName = "internalDate")]
        public string InternalDate { get; set; }
        [DataMember(Name = "payload")]
        [JsonProperty(PropertyName = "payload")]
        public EmailPayload Payload { get; set; }
        [DataMember(Name = "sizeEstimate")]
        [JsonProperty(PropertyName = "sizeEstimate")]
        public int SizeEstimate { get; set; }
    }
}
