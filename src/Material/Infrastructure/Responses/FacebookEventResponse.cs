using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundations.HttpClient.Metadata;
namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FacebookPlace
    {
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class FacebookEventDatum
    {
        [DataMember(Name = "description")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "start_time")]
        [JsonProperty(PropertyName = "start_time")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "rsvp_status")]
        [JsonProperty(PropertyName = "rsvp_status")]
        public string RsvpStatus { get; set; }
        [DataMember(Name = "place")]
        [JsonProperty(PropertyName = "place")]
        public FacebookPlace Place { get; set; }
    }

    [DatetimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
    [DataContract]
    public class FacebookEventResponse
    {
        [DataMember(Name = "data")]
        [JsonProperty(PropertyName = "data")]
        public IList<FacebookEventDatum> Data { get; set; }
        [DataMember(Name = "paging")]
        [JsonProperty(PropertyName = "paging")]
        public FacebookPaging Paging { get; set; }
    }
}
