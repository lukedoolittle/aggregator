using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class BingSpeechProperties
    {
        [DataMember(Name = "requestid")]
        public string Requestid { get; set; }

        [DataMember(Name = "HIGHCONF")]
        public string HighConfidence { get; set; }

        [DataMember(Name = "NOSPEECH")]
        public string NoSpeech { get; set; }

        [DataMember(Name = "FALSERECO")]
        public string FalseRecording { get; set; }

        [DataMember(Name = "MIDCONF")]
        public string MediumConfidence { get; set; }

        [DataMember(Name = "LOWCONF")]
        public string LowConfidence { get; set; }

        [DataMember(Name = "ERROR")]
        public string Error { get; set; }

    }

    [DataContract]
    public class BingSpeechHeader
    {

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "scenario")]
        public string Scenario { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "lexical")]
        public string Lexical { get; set; }

        [DataMember(Name = "properties")]
        public BingSpeechProperties Properties { get; set; }
    }

    [DataContract]
    public class BingSpeechToken
    {

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "lexical")]
        public string Lexical { get; set; }

        [DataMember(Name = "pronunciation")]
        public string Pronunciation { get; set; }
    }

    [DataContract]
    public class BingSpeechResult
    {

        [DataMember(Name = "scenario")]
        public string Scenario { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "lexical")]
        public string Lexical { get; set; }

        [DataMember(Name = "tokens")]
        public IList<BingSpeechToken> Tokens { get; set; }

        [DataMember(Name = "confidence")]
        public string Confidence { get; set; }

        [DataMember(Name = "properties")]
        public BingSpeechProperties SpeechProperties { get; set; }
    }

    [DataContract]
    public class MicrosoftBingSpeechResponse
    {

        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "header")]
        public BingSpeechHeader Header { get; set; }

        [DataMember(Name = "results")]
        public IList<BingSpeechResult> Results { get; set; }
    }
}
