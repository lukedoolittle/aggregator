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
    public class FitnessActivityItem
    {
        [DataMember(Name = "utc_offset")]
        [JsonProperty(PropertyName = "utc_offset")]
        public int UtcOffset { get; set; }
        [DataMember(Name = "duration")]
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [DataMember(Name = "start_time")]
        [JsonProperty(PropertyName = "start_time")]
        public string StartTime { get; set; }
        [DataMember(Name = "total_calories")]
        [JsonProperty(PropertyName = "total_calories")]
        public int TotalCalories { get; set; }
        [DataMember(Name = "tracking_mode")]
        [JsonProperty(PropertyName = "tracking_mode")]
        public string TrackingMode { get; set; }
        [DataMember(Name = "total_distance")]
        [JsonProperty(PropertyName = "total_distance")]
        public int TotalDistance { get; set; }
        [DataMember(Name = "entry_mode")]
        [JsonProperty(PropertyName = "entry_mode")]
        public string EntryMode { get; set; }
        [DataMember(Name = "has_path")]
        [JsonProperty(PropertyName = "has_path")]
        public bool HasPath { get; set; }
        [DataMember(Name = "source")]
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    [DataContract]
    public class RunkeeperFitnessActivityResponse
    {
        [DataMember(Name = "next")]
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }
        [DataMember(Name = "size")]
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<FitnessActivityItem> Items { get; set; }
    }


}
