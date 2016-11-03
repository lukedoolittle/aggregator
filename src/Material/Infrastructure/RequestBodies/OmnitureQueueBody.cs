using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Foundations.Attributes;
using Foundations.Extensions;

namespace Material.Infrastructure.RequestBodies
{
    [DataContract]
    public class OmnitureMetric
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [DataContract]
    public class OmnitureElement
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "selected", EmitDefaultValue = false)]
        public IList<string> Selected { get; set; }
    }

    [DataContract]
    public class OmnitureReportDescription
    {
        [DataMember(Name = "reportSuiteID")]
        public string ReportSuiteId { get; set; }

        public DateTime? Date { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "date", EmitDefaultValue = false)]
        private string _date
        {
            get { return Date?.ToString( 
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        public DateTime? StartDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "dateFrom", EmitDefaultValue = false)]
        private string _startDate
        {
            get { return StartDate?.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        public DateTime? EndDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "dateTo", EmitDefaultValue = false)]
        private string _endDate
        {
            get { return EndDate?.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        public OmnitureReportingDateGranularity DateGranularity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DataMember(Name = "dateGranularity")]
        private string _dateGranularity
        {
            get { return DateGranularity.EnumToString(); }
            set { throw new NotImplementedException(); }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "metrics")]
        public IList<OmnitureMetric> Metrics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public IList<OmnitureElement> Elements { get; set; }
    }

    [DataContract]
    public class OmnitureQueueBody
    {
        [DataMember(Name = "reportDescription")]
        public OmnitureReportDescription ReportDescription { get; set; }
    }

    [DataContract(Name = "OmnitureReportingDateGranularity")]
    public enum OmnitureReportingDateGranularity
    {
        [Description("hour")]
        Hour,
        [Description("day")]
        Day,
        [Description("week")]
        Week,
        [Description("month")]
        Month,
        [Description("quarter")]
        Quarter,
        [Description("year")]
        Year
    }
}
