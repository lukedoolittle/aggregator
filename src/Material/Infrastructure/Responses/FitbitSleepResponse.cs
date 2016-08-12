using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FitbitMinuteData
    {
        [DataMember(Name = "dateTime")]
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [DataMember(Name = "value")]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class FitbitSleepData
    {
        [DataMember(Name = "awakeCount")]
        [JsonProperty(PropertyName = "awakeCount")]
        public int AwakeCount { get; set; }
        [DataMember(Name = "awakeDuration")]
        [JsonProperty(PropertyName = "awakeDuration")]
        public int AwakeDuration { get; set; }
        [DataMember(Name = "awakeningsCount")]
        [JsonProperty(PropertyName = "awakeningsCount")]
        public int AwakeningsCount { get; set; }
        [DataMember(Name = "dateOfSleep")]
        [JsonProperty(PropertyName = "dateOfSleep")]
        public string DateOfSleep { get; set; }
        [DataMember(Name = "duration")]
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [DataMember(Name = "efficiency")]
        [JsonProperty(PropertyName = "efficiency")]
        public int Efficiency { get; set; }
        [DataMember(Name = "isMainSleep")]
        [JsonProperty(PropertyName = "isMainSleep")]
        public bool IsMainSleep { get; set; }
        [DataMember(Name = "logId")]
        [JsonProperty(PropertyName = "logId")]
        public long LogId { get; set; }
        [DataMember(Name = "minuteData")]
        [JsonProperty(PropertyName = "minuteData")]
        public IList<FitbitMinuteData> MinuteData { get; set; }
        [DataMember(Name = "minutesAfterWakeup")]
        [JsonProperty(PropertyName = "minutesAfterWakeup")]
        public int MinutesAfterWakeup { get; set; }
        [DataMember(Name = "minutesAsleep")]
        [JsonProperty(PropertyName = "minutesAsleep")]
        public int MinutesAsleep { get; set; }
        [DataMember(Name = "minutesAwake")]
        [JsonProperty(PropertyName = "minutesAwake")]
        public int MinutesAwake { get; set; }
        [DataMember(Name = "minutesToFallAsleep")]
        [JsonProperty(PropertyName = "minutesToFallAsleep")]
        public int MinutesToFallAsleep { get; set; }
        [DataMember(Name = "restlessCount")]
        [JsonProperty(PropertyName = "restlessCount")]
        public int RestlessCount { get; set; }
        [DataMember(Name = "restlessDuration")]
        [JsonProperty(PropertyName = "restlessDuration")]
        public int RestlessDuration { get; set; }
        [DataMember(Name = "startTime")]
        [JsonProperty(PropertyName = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "timeInBed")]
        [JsonProperty(PropertyName = "timeInBed")]
        public int TimeInBed { get; set; }
    }

    [DataContract]
    public class FitbitSleepSummary
    {
        [DataMember(Name = "totalMinutesAsleep")]
        [JsonProperty(PropertyName = "totalMinutesAsleep")]
        public int TotalMinutesAsleep { get; set; }
        [DataMember(Name = "totalSleepRecords")]
        [JsonProperty(PropertyName = "totalSleepRecords")]
        public int TotalSleepRecords { get; set; }
        [DataMember(Name = "totalTimeInBed")]
        [JsonProperty(PropertyName = "totalTimeInBed")]
        public int TotalTimeInBed { get; set; }
    }

    [DatetimeFormatter("yyyy-MM-ddTHH:mm:ss.fff")]
    [DataContract]
    public class FitbitSleepResponse
    {
        [DataMember(Name = "sleep")]
        [JsonProperty(PropertyName = "sleep")]
        public IList<FitbitSleepData> Sleep { get; set; }
        [DataMember(Name = "summary")]
        [JsonProperty(PropertyName = "summary")]
        public FitbitSleepSummary Summary { get; set; }
    }
}
