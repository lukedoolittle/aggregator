using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class FacebookPlace
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class FacebookEventDatum
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "rsvp_status")]
        public string RsvpStatus { get; set; }
        [JsonProperty(PropertyName = "place")]
        public FacebookPlace Place { get; set; }
    }

    public class FacebookEventResponse
    {
        [JsonProperty(PropertyName = "data")]
        public IList<FacebookEventDatum> Data { get; set; }
        [JsonProperty(PropertyName = "paging")]
        public FacebookPaging Paging { get; set; }
    }
}
