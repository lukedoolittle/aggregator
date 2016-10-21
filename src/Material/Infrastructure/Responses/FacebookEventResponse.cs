using System.Runtime.Serialization;
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
        public string Name { get; set; }
    }

    [DataContract]
    public class FacebookEventDatum
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "start_time")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "rsvp_status")]
        public string RsvpStatus { get; set; }
        [DataMember(Name = "place")]
        public FacebookPlace Place { get; set; }
    }

    [DateTimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
    [DataContract]
    public class FacebookEventResponse
    {
        [DataMember(Name = "data")]
        public IList<FacebookEventDatum> Data { get; set; }
        [DataMember(Name = "paging")]
        public FacebookPaging Paging { get; set; }
    }
}
