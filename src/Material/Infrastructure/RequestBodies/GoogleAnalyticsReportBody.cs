using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Material.Infrastructure.RequestBodies
{
    //https://developers.google.com/analytics/devguides/reporting/core/v4/rest/v4/reports/batchGet
    //https://developers.google.com/analytics/devguides/reporting/core/v4/

    [DataContract]
    public class GoogleAnalyticsReportBody
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "reportRequests")]
        public List<GoogleAnalyticsReportRequest> ReportRequests { get; set; }
    }

    [DataContract]
    public class GoogleAnalyticsReportRequest
    {
        [DataMember(Name = "viewId", Order = 1)]
        public string ViewId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "dateRanges", Order = 2, EmitDefaultValue = false)]
        public List<GoogleAnalyticsDateRange> DateRanges { get; set; }

        public SamplingLevel? SamplingLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "samplingLevel", EmitDefaultValue = false)]
        private string _samplingLevel
        {
            get { return SamplingLevel?.ToString(); }
            set { throw new NotImplementedException(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "dimensions", Order = 3, EmitDefaultValue = false)]
        public List<GoogleAnalyticsDimension> Dimensions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "metrics", Order = 4, EmitDefaultValue = false)]
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

    public enum SamplingLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SAMPLING")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UNSPECIFIED")]
        SAMPLING_UNSPECIFIED,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DEFAULT")]
        DEFAULT,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SMALL")]
        SMALL,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "LARGE")]
        LARGE
    }

    [DataContract]
    public class GoogleAnalyticsDateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "startDate", Order = 1)]
        private string _startDate
        {
            get { return StartDate.ToString(
                "yyyy-MM-dd", 
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "endDate", Order = 2)]
        private string _endDate
        {
            get { return EndDate.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
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

        public MetricType? FormattingType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "formattingType", EmitDefaultValue = false)]
        private string _formattingType
        {
            get { return FormattingType?.ToString(); }
            set { throw new NotImplementedException(); }
        }
    }

    public enum MetricType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UNSPECIFIED")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TYPE")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "METRIC")]
        METRIC_TYPE_UNSPECIFIED,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INTEGER")]
        INTEGER,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FLOAT")]
        FLOAT,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CURRENCY")]
        CURRENCY,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PERCENT")]
        PERCENT,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TIME")]
        TIME
    }

    [DataContract]
    public class GoogleAnalyticsDimension
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "expression", EmitDefaultValue = false)]
        public List<string> HistogramBuckets { get; set; }
    }
}
