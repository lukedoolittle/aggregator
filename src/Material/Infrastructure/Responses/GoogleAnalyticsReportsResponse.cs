using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsMetricHeaderEntry
    {

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsMetricHeader
    {

        [DataMember(Name = "metricHeaderEntries")]
        public IList<GoogleAnalyticsReportsMetricHeaderEntry> MetricHeaderEntries { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsColumnHeader
    {

        [DataMember(Name = "metricHeader")]
        public GoogleAnalyticsReportsMetricHeader MetricHeader { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsMetric
    {

        [DataMember(Name = "values")]
        public IList<string> Values { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsRow
    {
        [DataMember(Name = "dimensions")]
        public IList<string> Dimensions { get; set; }

        [DataMember(Name = "metrics")]
        public IList<GoogleAnalyticsReportsMetric> Metrics { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsTotal
    {
        [DataMember(Name = "values")]
        public IList<string> Values { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsMinimum
    {

        [DataMember(Name = "values")]
        public IList<string> Values { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsMaximum
    {

        [DataMember(Name = "values")]
        public IList<string> Values { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsData
    {

        [DataMember(Name = "rows")]
        public IList<GoogleAnalyticsRow> Rows { get; set; }

        [DataMember(Name = "totals")]
        public IList<GoogleAnalyticsReportsTotal> Totals { get; set; }

        [DataMember(Name = "rowCount")]
        public int RowCount { get; set; }

        [DataMember(Name = "minimums")]
        public IList<GoogleAnalyticsReportsMinimum> Minimums { get; set; }

        [DataMember(Name = "maximums")]
        public IList<GoogleAnalyticsReportsMaximum> Maximums { get; set; }

        [DataMember(Name = "samplesReadCounts")]
        public IList<string> SamplesReadCounts { get; set; }

        [DataMember(Name = "samplingSpaceSizes")]
        public IList<string> SamplingSpaceSizes { get; set; }

        [DataMember(Name = "isDataGolden")]
        public bool IsDataGolden { get; set; }

    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReport
    {

        [DataMember(Name = "columnHeader")]
        public GoogleAnalyticsReportsColumnHeader ColumnHeader { get; set; }

        [DataMember(Name = "data")]
        public GoogleAnalyticsReportsData Data { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class GoogleAnalyticsReportsResponse
    {

        [DataMember(Name = "reports")]
        public IList<GoogleAnalyticsReport> Reports { get; set; }
    }


}
