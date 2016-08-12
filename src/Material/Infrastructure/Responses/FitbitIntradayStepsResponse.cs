using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FitbitActivitiesStep
    {
        [DataMember(Name = "dateTime")]
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [DataMember(Name = "value")]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class FitbitStepsDataset
    {
        [DataMember(Name = "time")]
        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
        [DataMember(Name = "value")]
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }

    [DataContract]
    public class ActivitiesStepsIntraday
    {
        [DataMember(Name = "dataset")]
        [JsonProperty(PropertyName = "dataset")]
        public IList<FitbitStepsDataset> Dataset { get; set; }
        [DataMember(Name = "datasetInterval")]
        [JsonProperty(PropertyName = "datasetInterval")]
        public int DatasetInterval { get; set; }
        [DataMember(Name = "datasetType")]
        [JsonProperty(PropertyName = "datasetType")]
        public string DatasetType { get; set; }
    }

    [DataContract]
    public class FitbitIntradayStepsResponse
    {
        [JsonProperty(PropertyName = "activities-steps")]
        public IList<FitbitActivitiesStep> ActivitiesSteps { get; set; }

        [JsonProperty(PropertyName = "activities-steps-intraday")]
        public ActivitiesStepsIntraday ActivitiesStepsIntraday { get; set; }
    }

}
