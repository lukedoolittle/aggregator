using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Foundations.Attributes;
using Foundations.Extensions;

namespace Material.Infrastructure.RequestBodies
{
    [DataContract]
    public class Metric
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [DataContract]
    public class Element
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "selected", EmitDefaultValue = false)]
        public IList<string> Selected { get; set; }
    }

    [DataContract]
    public class ReportDescription
    {
        [DataMember(Name = "reportSuiteID")]
        public string ReportSuiteId { get; set; }

        public DateTime? Date { get; set; }
        [DataMember(Name = "date", EmitDefaultValue = false)]
        private string _date
        {
            get { return Date?.ToString("yyyy-MM-dd"); }
            set { throw new Exception(); }
        }

        public DateTime? StartDate { get; set; }
        [DataMember(Name = "dateFrom", EmitDefaultValue = false)]
        private string _startDate
        {
            get { return StartDate?.ToString("yyyy-MM-dd"); }
            set { throw new Exception(); }
        }

        public DateTime? EndDate { get; set; }
        [DataMember(Name = "dateTo", EmitDefaultValue = false)]
        private string _endDate
        {
            get { return EndDate?.ToString("yyyy-MM-dd"); }
            set { throw new Exception(); }
        }

        //TODO: correct this with a custom serializer
        public OmnitureReportingDateGranularityEnum DateGranularity { get; set; }

        [DataMember(Name = "dateGranularity")]
        private string _dateGranularity
        {
            get { return DateGranularity.EnumToString(); }
            set { throw new Exception(); }
        }
        [DataMember(Name = "metrics")]
        public IList<Metric> Metrics { get; set; }
        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public IList<Element> Elements { get; set; }
    }

    [DataContract]
    public class OmnitureQueueBody
    {
        [DataMember(Name = "reportDescription")]
        public ReportDescription ReportDescription { get; set; }
    }

    [DataContract(Name = "OmnitureReportingDateGranularity")]
    public enum OmnitureReportingDateGranularityEnum
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
