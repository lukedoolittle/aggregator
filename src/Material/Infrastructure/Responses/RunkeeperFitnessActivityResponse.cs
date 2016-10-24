using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FitnessActivityItem
    {
        [DataMember(Name = "utc_offset")]
        public int UtcOffset { get; set; }
        [DataMember(Name = "duration")]
        public int Duration { get; set; }
        [DataMember(Name = "start_time")]
        public string StartTime { get; set; }
        [DataMember(Name = "total_calories")]
        public int TotalCalories { get; set; }
        [DataMember(Name = "tracking_mode")]
        public string TrackingMode { get; set; }
        [DataMember(Name = "total_distance")]
        public int TotalDistance { get; set; }
        [DataMember(Name = "entry_mode")]
        public string EntryMode { get; set; }
        [DataMember(Name = "has_path")]
        public bool HasPath { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
    }

    [DataContract]
    public class RunkeeperFitnessActivityResponse
    {
        [DataMember(Name = "next")]
        public string Next { get; set; }
        [DataMember(Name = "size")]
        public int Size { get; set; }
        [DataMember(Name = "items")]
        public IList<FitnessActivityItem> Items { get; set; }
    }


}
