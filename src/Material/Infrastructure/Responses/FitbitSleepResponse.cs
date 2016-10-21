using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FitbitMinuteData
    {
        [DataMember(Name = "dateTime")]
        public string DateTime { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class FitbitSleepData
    {
        [DataMember(Name = "awakeCount")]
        public int AwakeCount { get; set; }
        [DataMember(Name = "awakeDuration")]
        public int AwakeDuration { get; set; }
        [DataMember(Name = "awakeningsCount")]
        public int AwakeningsCount { get; set; }
        [DataMember(Name = "dateOfSleep")]
        public string DateOfSleep { get; set; }
        [DataMember(Name = "duration")]
        public int Duration { get; set; }
        [DataMember(Name = "efficiency")]
        public int Efficiency { get; set; }
        [DataMember(Name = "isMainSleep")]
        public bool IsMainSleep { get; set; }
        [DataMember(Name = "logId")]
        public long LogId { get; set; }
        [DataMember(Name = "minuteData")]
        public IList<FitbitMinuteData> MinuteData { get; set; }
        [DataMember(Name = "minutesAfterWakeup")]
        public int MinutesAfterWakeup { get; set; }
        [DataMember(Name = "minutesAsleep")]
        public int MinutesAsleep { get; set; }
        [DataMember(Name = "minutesAwake")]
        public int MinutesAwake { get; set; }
        [DataMember(Name = "minutesToFallAsleep")]
        public int MinutesToFallAsleep { get; set; }
        [DataMember(Name = "restlessCount")]
        public int RestlessCount { get; set; }
        [DataMember(Name = "restlessDuration")]
        public int RestlessDuration { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "timeInBed")]
        public int TimeInBed { get; set; }
    }

    [DataContract]
    public class FitbitSleepSummary
    {
        [DataMember(Name = "totalMinutesAsleep")]
        public int TotalMinutesAsleep { get; set; }
        [DataMember(Name = "totalSleepRecords")]
        public int TotalSleepRecords { get; set; }
        [DataMember(Name = "totalTimeInBed")]
        public int TotalTimeInBed { get; set; }
    }

    [DateTimeFormatter("yyyy-MM-ddTHH:mm:ss.fff")]
    [DataContract]
    public class FitbitSleepResponse
    {
        [DataMember(Name = "sleep")]
        public IList<FitbitSleepData> Sleep { get; set; }
        [DataMember(Name = "summary")]
        public FitbitSleepSummary Summary { get; set; }
    }
}
