using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FitbitMinuteData
    {
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class FitbitSleepData
    {
        [JsonProperty(PropertyName = "awakeCount")]
        public int AwakeCount { get; set; }
        [JsonProperty(PropertyName = "awakeDuration")]
        public int AwakeDuration { get; set; }
        [JsonProperty(PropertyName = "awakeningsCount")]
        public int AwakeningsCount { get; set; }
        [JsonProperty(PropertyName = "dateOfSleep")]
        public string DateOfSleep { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [JsonProperty(PropertyName = "efficiency")]
        public int Efficiency { get; set; }
        [JsonProperty(PropertyName = "isMainSleep")]
        public bool IsMainSleep { get; set; }
        [JsonProperty(PropertyName = "logId")]
        public long LogId { get; set; }
        [JsonProperty(PropertyName = "minuteData")]
        public IList<FitbitMinuteData> MinuteData { get; set; }
        [JsonProperty(PropertyName = "minutesAfterWakeup")]
        public int MinutesAfterWakeup { get; set; }
        [JsonProperty(PropertyName = "minutesAsleep")]
        public int MinutesAsleep { get; set; }
        [JsonProperty(PropertyName = "minutesAwake")]
        public int MinutesAwake { get; set; }
        [JsonProperty(PropertyName = "minutesToFallAsleep")]
        public int MinutesToFallAsleep { get; set; }
        [JsonProperty(PropertyName = "restlessCount")]
        public int RestlessCount { get; set; }
        [JsonProperty(PropertyName = "restlessDuration")]
        public int RestlessDuration { get; set; }
        [JsonProperty(PropertyName = "startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "timeInBed")]
        public int TimeInBed { get; set; }
    }

    public class FitbitSleepSummary
    {
        [JsonProperty(PropertyName = "totalMinutesAsleep")]
        public int TotalMinutesAsleep { get; set; }
        [JsonProperty(PropertyName = "totalSleepRecords")]
        public int TotalSleepRecords { get; set; }
        [JsonProperty(PropertyName = "totalTimeInBed")]
        public int TotalTimeInBed { get; set; }
    }

    public class FitbitSleepResponse
    {
        [JsonProperty(PropertyName = "sleep")]
        public IList<FitbitSleepData> Sleep { get; set; }
        [JsonProperty(PropertyName = "summary")]
        public FitbitSleepSummary Summary { get; set; }
    }
}
