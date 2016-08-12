using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
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

    public class HourBlock : List<string>
    {
        public DateTimeOffset Date => DateTimeOffset.ParseExact(this[0], "yyyy-MM-ddTHH:mm:ss", null);
        public string TimeSpentInSeconds => this[1];
        public string NumberOfPeople => this[2];
        public string EfficiencyNumber => this[3];
        public string EfficiencyPercent => this[4];
    }
}
