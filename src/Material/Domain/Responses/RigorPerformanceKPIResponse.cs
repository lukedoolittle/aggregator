using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class RigorPage
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "hostname")]
        public string Hostname { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class RigorDatum
    {
        [DataMember(Name = "from")]
        public string From { get; set; }

        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "value")]
        public int Value { get; set; }

        [DataMember(Name = "run_id")]
        public object RunId { get; set; }

        [DataMember(Name = "page_history_id")]
        public int PageHistoryId { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class RigorSeries
    {
        [DataMember(Name = "page")]
        public RigorPage Page { get; set; }

        [DataMember(Name = "metric")]
        public RigorMetric Metric { get; set; }

        [DataMember(Name = "data")]
        public IList<RigorDatum> Data { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class RigorMetric
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "unit")]
        public string Unit { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class RigorPerformanceKpiResponse
    {
        [DataMember(Name = "check_id")]
        public int CheckId { get; set; }

        [DataMember(Name = "from")]
        public string From { get; set; }

        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "range")]
        public string Range { get; set; }

        [DataMember(Name = "series")]
        public IList<RigorSeries> Series { get; set; }

        [DataMember(Name = "metrics")]
        public IList<RigorMetric> Metrics { get; set; }
    }
}
