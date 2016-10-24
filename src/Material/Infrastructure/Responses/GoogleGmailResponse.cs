using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class EmailBody
    {
        [DataMember(Name = "size")]
        public int Size { get; set; }
        [DataMember(Name = "data")]
        public string Data { get; set; }
    }

    [DataContract]
    public class EmailHeader
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class EmailPart
    {
        [DataMember(Name = "partId")]
        public string PartId { get; set; }
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }
        [DataMember(Name = "filename")]
        public string Filename { get; set; }
        [DataMember(Name = "headers")]
        public IList<EmailHeader> Headers { get; set; }
        [DataMember(Name = "body")]
        public EmailBody Body { get; set; }
    }

    [DataContract]
    public class EmailPayload
    {
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }
        [DataMember(Name = "filename")]
        public string Filename { get; set; }
        [DataMember(Name = "headers")]
        public IList<EmailHeader> Headers { get; set; }
        [DataMember(Name = "body")]
        public EmailBody Body { get; set; }
        [DataMember(Name = "parts")]
        public IList<EmailPart> Parts { get; set; }
    }

    [DataContract]
    public class GoogleGmailResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "threadId")]
        public string ThreadId { get; set; }
        [DataMember(Name = "labelIds")]
        public IList<string> LabelIds { get; set; }
        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }
        [DataMember(Name = "historyId")]
        public string HistoryId { get; set; }
        [DataMember(Name = "internalDate")]
        public string InternalDate { get; set; }
        [DataMember(Name = "payload")]
        public EmailPayload Payload { get; set; }
        [DataMember(Name = "sizeEstimate")]
        public int SizeEstimate { get; set; }
    }
}
