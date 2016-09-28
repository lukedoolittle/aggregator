using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class GoogleAnalyticsReportsMetricHeaderEntry
    {

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsMetricHeader
    {

        [DataMember(Name = "metricHeaderEntries")]
        public IList<GoogleAnalyticsReportsMetricHeaderEntry> metricHeaderEntries { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsColumnHeader
    {

        [DataMember(Name = "metricHeader")]
        public GoogleAnalyticsReportsMetricHeader metricHeader { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsMetric
    {

        [DataMember(Name = "values")]
        public IList<string> values { get; set; }
    }

    [DataContract]
    public class Row
    {

        [DataMember(Name = "metrics")]
        public IList<GoogleAnalyticsReportsMetric> metrics { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsTotal
    {

        [DataMember(Name = "values")]
        public IList<string> values { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsMinimum
    {

        [DataMember(Name = "values")]
        public IList<string> values { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsMaximum
    {

        [DataMember(Name = "values")]
        public IList<string> values { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsData
    {

        [DataMember(Name = "rows")]
        public IList<Row> rows { get; set; }

        [DataMember(Name = "totals")]
        public IList<GoogleAnalyticsReportsTotal> totals { get; set; }

        [DataMember(Name = "rowCount")]
        public int rowCount { get; set; }

        [DataMember(Name = "minimums")]
        public IList<GoogleAnalyticsReportsMinimum> minimums { get; set; }

        [DataMember(Name = "maximums")]
        public IList<GoogleAnalyticsReportsMaximum> maximums { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReport
    {

        [DataMember(Name = "columnHeader")]
        public GoogleAnalyticsReportsColumnHeader columnHeader { get; set; }

        [DataMember(Name = "data")]
        public GoogleAnalyticsReportsData data { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportsResponse
    {

        [DataMember(Name = "reports")]
        public IList<Report> reports { get; set; }
    }


}
