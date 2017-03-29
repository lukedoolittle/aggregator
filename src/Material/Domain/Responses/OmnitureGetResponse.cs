using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Element
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class ReportSuite
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Metric
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "decimals")]
        public int Decimals { get; set; }
        [DataMember(Name = "latency")]
        public int Latency { get; set; }
        [DataMember(Name = "current")]
        public bool Current { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Datum
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "year")]
        public int Year { get; set; }
        [DataMember(Name = "month")]
        public int Month { get; set; }
        [DataMember(Name = "day")]
        public int Day { get; set; }
        [DataMember(Name = "hour")]
        public int Hour { get; set; }
        [DataMember(Name = "counts")]
        public IList<string> Counts { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Report
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "elements")]
        public IList<Element> Elements { get; set; }
        [DataMember(Name = "reportSuite")]
        public ReportSuite ReportSuite { get; set; }
        [DataMember(Name = "period")]
        public string Period { get; set; }
        [DataMember(Name = "metrics")]
        public IList<Metric> Metrics { get; set; }
        [DataMember(Name = "data")]
        public IList<Datum> Data { get; set; }
        [DataMember(Name = "totals")]
        public IList<string> Totals { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureGetResponse
    {
        [DataMember(Name = "report")]
        public Report Report { get; set; }
        [DataMember(Name = "waitSeconds")]
        public int WaitSeconds { get; set; }
        [DataMember(Name = "runSeconds")]
        public int RunSeconds { get; set; }
    }
}


