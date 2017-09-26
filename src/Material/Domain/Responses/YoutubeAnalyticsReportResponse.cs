using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class ColumnHeader
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "columnType")]
        public string ColumnType { get; set; }

        [DataMember(Name = "dataType")]
        public string DataType { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class YoutubeAnalyticsReportResponse
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "columnHeaders")]
        public IList<ColumnHeader> ColumnHeaders { get; set; }

        [DataMember(Name = "rows")]
        public IList<IList<string>> Rows { get; set; }

        public IList<IDictionary<string, string>> GetResults()
        {
            var results = new List<IDictionary<string, string>>();

            foreach (var row in Rows)
            {
                var result = new Dictionary<string, string>();
                for (var i = 0; i < ColumnHeaders.Count; i++)
                {
                    result.Add(ColumnHeaders[i].Name, row[i]);
                }
                results.Add(result);
            }

            return results;
        }
    }
}
