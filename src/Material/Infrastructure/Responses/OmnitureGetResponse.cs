//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Material.Infrastructure.Responses
//{
//    public class Element
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class ReportSuite
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class Metric
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public string type { get; set; }
//        public int decimals { get; set; }
//        public int latency { get; set; }
//        public bool current { get; set; }
//    }

//    public class Breakdown
//    {
//        public string name { get; set; }
//        public string url { get; set; }
//        public IList<string> counts { get; set; }
//    }

//    public class Breakdown
//    {
//        public string name { get; set; }
//        public string url { get; set; }
//        public IList<string> counts { get; set; }
//        public IList<Breakdown> breakdown { get; set; }
//        public IList<string> breakdownTotal { get; set; }
//    }

//    public class Datum
//    {
//        public string name { get; set; }
//        public int year { get; set; }
//        public int month { get; set; }
//        public int day { get; set; }
//        public int hour { get; set; }
//        public IList<Breakdown> breakdown { get; set; }
//        public IList<string> breakdownTotal { get; set; }
//    }

//    public class Report
//    {
//        public string type { get; set; }
//        public IList<Element> elements { get; set; }
//        public ReportSuite reportSuite { get; set; }
//        public string period { get; set; }
//        public IList<Metric> metrics { get; set; }
//        public IList<Datum> data { get; set; }
//        public IList<string> totals { get; set; }
//        public string version { get; set; }
//    }

//    public class OmnitureGetResponse
//    {
//        public Report report { get; set; }
//        public int waitSeconds { get; set; }
//        public int runSeconds { get; set; }
//    }
//}

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class Element
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class ReportSuite
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

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


