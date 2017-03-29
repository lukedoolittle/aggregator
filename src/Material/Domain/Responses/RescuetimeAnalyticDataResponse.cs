using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class RescuetimeAnalyticDataResponse
    {
        [DataMember(Name = "notes")]
        public string Notes { get; set; }
        [DataMember(Name = "row_headers")]
        public IList<string> RowHeaders { get; set; }
        [DataMember(Name = "rows")]
        public IList<HourBlock> Rows { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    public class HourBlock : List<string>
    {
        public DateTimeOffset Date => DateTimeOffset.ParseExact(this[0], "yyyy-MM-ddTHH:mm:ss", null);
        public string TimeSpentInSeconds => this[1];
        public string NumberOfPeople => this[2];
        public string EfficiencyNumber => this[3];
        public string EfficiencyPercent => this[4];
    }
}
