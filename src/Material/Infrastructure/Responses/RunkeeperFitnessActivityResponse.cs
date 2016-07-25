using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class FitnessActivityItem
    {
        [JsonProperty(PropertyName = "utc_offset")]
        public int UtcOffset { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [JsonProperty(PropertyName = "start_time")]
        public string StartTime { get; set; }
        [JsonProperty(PropertyName = "total_calories")]
        public int TotalCalories { get; set; }
        [JsonProperty(PropertyName = "tracking_mode")]
        public string TrackingMode { get; set; }
        [JsonProperty(PropertyName = "total_distance")]
        public int TotalDistance { get; set; }
        [JsonProperty(PropertyName = "entry_mode")]
        public string EntryMode { get; set; }
        [JsonProperty(PropertyName = "has_path")]
        public bool HasPath { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    public class RunkeeperFitnessActivityResponse
    {
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<FitnessActivityItem> Items { get; set; }
    }


}
