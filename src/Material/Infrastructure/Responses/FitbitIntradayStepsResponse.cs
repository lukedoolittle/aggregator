using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Material.Infrastructure.Responses
{
    public class FitbitActivitiesStep
    {
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class FitbitStepsDataset
    {
        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }

    public class ActivitiesStepsIntraday
    {
        [JsonProperty(PropertyName = "dataset")]
        public IList<FitbitStepsDataset> Dataset { get; set; }
        [JsonProperty(PropertyName = "datasetInterval")]
        public int DatasetInterval { get; set; }
        [JsonProperty(PropertyName = "datasetType")]
        public string DatasetType { get; set; }
    }

    public class FitbitIntradayStepsResponse
    {
        [JsonProperty(PropertyName = "activities-steps")]
        public IList<FitbitActivitiesStep> ActivitiesSteps { get; set; }

        [JsonProperty(PropertyName = "activities-steps-intraday")]
        public ActivitiesStepsIntraday ActivitiesStepsIntraday { get; set; }
    }

}
