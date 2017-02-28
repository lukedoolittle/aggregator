using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Row
    {
        [DataMember(Name = "dateTime")]
        public string DateTime { get; set; }

        [DataMember(Name = "company|name")]
        public string Company { get; set; }

        [DataMember(Name = "app|name")]
        public string Application { get; set; }

        [DataMember(Name = "appVersion|name")]
        public string ApplicationVersion { get; set; }

        [DataMember(Name = "country|name")]
        public string Country { get; set; }

        [DataMember(Name = "language|name")]
        public string Language { get; set; }

        [DataMember(Name = "region|name")]
        public string Region { get; set; }

        [DataMember(Name = "category|name")]
        public string Category { get; set; }

        [DataMember(Name = "sessions")]
        public int Sessions { get; set; }

        [DataMember(Name = "activeDevices")]
        public int ActiveDevices { get; set; }

        [DataMember(Name = "newDevices")]
        public int NewDevices { get; set; }

        [DataMember(Name = "timeSpent")]
        public int TimeSpent { get; set; }

        [DataMember(Name = "averageTimePerDevice")]
        public int AverageTimePerDevice { get; set; }

        [DataMember(Name = "averageTimePerSession")]
        public int AverageTimePerSession { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class YahooFlurryMetricsResponse
    {
        [DataMember(Name = "rows")]
        public IList<Row> Rows { get; set; }
    }
}
