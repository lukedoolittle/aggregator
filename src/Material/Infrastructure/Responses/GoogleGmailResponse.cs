using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class EmailBody
    {
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }

    public class EmailHeader
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class EmailPart
    {
        [JsonProperty(PropertyName = "partId")]
        public string PartId { get; set; }
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType { get; set; }
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
        [JsonProperty(PropertyName = "headers")]
        public IList<EmailHeader> Headers { get; set; }
        [JsonProperty(PropertyName = "body")]
        public EmailBody Body { get; set; }
    }

    public class EmailPayload
    {
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType { get; set; }
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
        [JsonProperty(PropertyName = "headers")]
        public IList<EmailHeader> Headers { get; set; }
        [JsonProperty(PropertyName = "body")]
        public EmailBody Body { get; set; }
        [JsonProperty(PropertyName = "parts")]
        public IList<EmailPart> Parts { get; set; }
    }

    public class GoogleGmailResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "threadId")]
        public string ThreadId { get; set; }
        [JsonProperty(PropertyName = "labelIds")]
        public IList<string> LabelIds { get; set; }
        [JsonProperty(PropertyName = "snippet")]
        public string Snippet { get; set; }
        [JsonProperty(PropertyName = "historyId")]
        public string HistoryId { get; set; }
        [JsonProperty(PropertyName = "internalDate")]
        public string InternalDate { get; set; }
        [JsonProperty(PropertyName = "payload")]
        public EmailPayload Payload { get; set; }
        [JsonProperty(PropertyName = "sizeEstimate")]
        public int SizeEstimate { get; set; }
    }
}
