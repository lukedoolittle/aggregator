using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookValue
    {
        [DataMember(Name = "value")]
        public int Value { get; set; }

        public DateTimeOffset Date => DateTimeOffset.ParseExact(EndTime, "yyyy-MM-ddTHH:mm:sszzz", null);

        [DataMember(Name = "end_time")]
        public string EndTime { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookPageDatum
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "period")]
        public string Period { get; set; }

        [DataMember(Name = "values")]
        public IList<FacebookValue> Values { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [DataContract]
    public class FacebookPreviousNextPaging
    {
        [DataMember(Name = "previous")]
        public string Previous { get; set; }

        [DataMember(Name = "next")]
        public string Next { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookPageInsightsResponse
    {
        [DataMember(Name = "data")]
        public IList<FacebookPageDatum> Data { get; set; }

        [DataMember(Name = "paging")]
        public FacebookPreviousNextPaging Paging { get; set; }
    }
}