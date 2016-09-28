using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.RequestBodies
{
    //https://developers.google.com/analytics/devguides/reporting/core/v4/rest/v4/reports/batchGet
    //https://developers.google.com/analytics/devguides/reporting/core/v4/

    [DataContract]
    public class GoogleAnalyticsReportBody
    {
        [DataMember(Name = "reportRequests")]
        public List<GoogleAnalyticsReportRequest> ReportRequests { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportRequest
    {
        [DataMember(Name = "viewId")]
        public string ViewId { get; set; }

        [DataMember(Name = "dateRanges", EmitDefaultValue = false)]
        public List<GoogleAnalyticsDateRange> DateRanges { get; set; }

        public SamplingLevelEnum? SamplingLevel { get; set; }
        [DataMember(Name = "samplingLevel", EmitDefaultValue = false)]
        private string _samplingLevel
        {
            get { return SamplingLevel?.ToString(); }
            set { throw new NotImplementedException(); }
        }

        [DataMember(Name = "dimensions", EmitDefaultValue = false)]
        public List<GoogleAnalyticsDimension> Dimensions { get; set; }

        [DataMember(Name = "metrics", EmitDefaultValue = false)]
        public List<GoogleAnalyticsMetric> Metrics { get; set; }

        [DataMember(Name = "filtersExpression", EmitDefaultValue = false)]
        public string FiltersExpression { get; set; }

        //metricFilterClauses
        //dimensionFilterClauses
        //orderbys
        //segments
        //pivots
        //cohortGroup

        [DataMember(Name = "pageToken", EmitDefaultValue = false)]
        public string PageToken { get; set; }

        [DataMember(Name = "pageSize", EmitDefaultValue = false)]
        public int? PageSize { get; set; }

        [DataMember(Name = "includeEmptyRows", EmitDefaultValue = false)]
        public bool? IncludeEmptyRows { get; set; }

        [DataMember(Name = "hideTotals", EmitDefaultValue = false)]
        public bool? HideTotals { get; set; }

        [DataMember(Name = "hideValueRanges", EmitDefaultValue = false)]
        public bool? HideValueRanges { get; set; }
    }

    public enum SamplingLevelEnum
    {
        SAMPLING_UNSPECIFIED,
        DEFAULT,
        SMALL,
        LARGE
    }

    [DataContract]
    public class GoogleAnalyticsDateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [DataMember(Name = "startDate")]
        private string _startDate
        {
            get { return StartDate.ToString("yyyy-MM-dd"); }
            set { throw new NotImplementedException(); }
        }

        [DataMember(Name = "endDate")]
        private string _endDate
        {
            get { return EndDate.ToString("yyyy-MM-dd"); }
            set { throw new NotImplementedException(); }
        }
    }

    [DataContract]
    public class GoogleAnalyticsMetric
    {
        [DataMember(Name = "expression")]
        public string Expression { get; set; }

        [DataMember(Name = "alias", EmitDefaultValue = false)]
        public string Alias { get; set; }

        public MetricTypeEnum? FormattingType { get; set; }

        [DataMember(Name = "formattingType", EmitDefaultValue = false)]
        private string _formattingType
        {
            get { return FormattingType?.ToString(); }
            set { throw new NotImplementedException(); }
        }
    }

    public enum MetricTypeEnum
    {
        METRIC_TYPE_UNSPECIFIED,
        INTEGER,
        FLOAT,
        CURRENCY,
        PERCENT,
        TIME
    }

    [DataContract]
    public class GoogleAnalyticsDimension
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "expression", EmitDefaultValue = false)]
        public List<string> HistogramBuckets { get; set; }
    }
}
