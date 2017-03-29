using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Material.Framework.Extensions;
using Material.Framework.Metadata;

namespace Material.Domain.RequestBodies
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureMetric
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureElement
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "selected", EmitDefaultValue = false)]
        public IList<string> Selected { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureReportDescription
    {
        [DataMember(Name = "reportSuiteID")]
        public string ReportSuiteId { get; set; }

        public DateTime? Date { get; set; }
        [DataMember(Name = "date", EmitDefaultValue = false)]
        private string _date
        {
            get { return Date?.ToString( 
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        public DateTime? StartDate { get; set; }
        [DataMember(Name = "dateFrom", EmitDefaultValue = false)]
        private string _startDate
        {
            get { return StartDate?.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        public DateTime? EndDate { get; set; }

        [DataMember(Name = "dateTo", EmitDefaultValue = false)]
        private string _endDate
        {
            get { return EndDate?.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture); }
            set { throw new NotImplementedException(); }
        }

        public OmnitureReportingDateGranularity DateGranularity { get; set; }

        [DataMember(Name = "dateGranularity")]
        private string _dateGranularity
        {
            get { return DateGranularity.EnumToString(); }
            set { throw new NotImplementedException(); }
        }

        [DataMember(Name = "metrics")]
        public IList<OmnitureMetric> Metrics { get; set; }

        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public IList<OmnitureElement> Elements { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureQueueBody
    {
        [DataMember(Name = "reportDescription")]
        public OmnitureReportDescription ReportDescription { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
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
