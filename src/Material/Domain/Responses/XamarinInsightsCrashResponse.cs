using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinInsightsStackTrace
    {
        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "exceptionType")]
        public string ExceptionType { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinInsightsCrashMetadata
    {
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinInsightsIssue
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "lastOccurredForUser")]
        public int? LastOccurredForUser { get; set; }

        [DataMember(Name = "stackTrace")]
        public XamarinInsightsStackTrace StackTrace { get; set; }

        [DataMember(Name = "state")]
        public int State { get; set; }

        [DataMember(Name = "reportType")]
        public int ReportType { get; set; }

        [DataMember(Name = "metadata")]
        public XamarinInsightsCrashMetadata Metadata { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinInsightsCrashesResponse
    {
        [DataMember(Name = "issues")]
        public IList<XamarinInsightsIssue> Issues { get; set; }

        [DataMember(Name = "openIssueErrorsCount")]
        public int OpenIssueErrorsCount { get; set; }

        [DataMember(Name = "openIssueWarningsCount")]
        public int OpenIssueWarningsCount { get; set; }
    }
}
